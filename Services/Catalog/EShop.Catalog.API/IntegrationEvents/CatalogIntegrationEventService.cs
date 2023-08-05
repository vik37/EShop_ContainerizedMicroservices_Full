namespace EShop.Catalog.API.IntegrationEvents;

public class CatalogIntegrationEventService : ICatalogIntegrationEventService, IDisposable
{
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly ILogger<CatalogIntegrationEventService> _logger;
    private readonly IEventBus _eventBus;
    private readonly CatalogDbContext _catalogContext;
    private readonly IIntegrationEventLogService _integrationEventLogService;
    private volatile bool dispossedValue;

    public CatalogIntegrationEventService(
            ILogger<CatalogIntegrationEventService> logger,
            IEventBus eventBus,
            CatalogDbContext catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        _integrationEventLogServiceFactory
            = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _integrationEventLogService = _integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
    }

    public async Task PublishThroughtEventBusAsync(IntegrationEvent integrationEvent)
    {
        try
        {
            _logger.LogInformation("--------- Publishing integration event {IntegrationEventId_Published} from {AppName} - {@(IntegrationEvent)}",
                integrationEvent.Id, CatalogApplication.GetApplication().ApplicationName, integrationEvent);

            await _integrationEventLogService.MarkEventAsInProgressAsync(integrationEvent.Id);
            _eventBus.Publish(integrationEvent);
            await _integrationEventLogService.MarkEventAsPublishedAsync(integrationEvent.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR: Publishing integration event: {IntegrationEventId_Published} from {AppName} - ({@IntegrationEvent})", integrationEvent.Id, CatalogApplication.GetApplication().ApplicationName, CatalogApplication.GetApplication().AppNamespace);
            await _integrationEventLogService.MarkEventAsFailedAsync(integrationEvent.Id);
        }
    }

    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent integrationEvent)
    {
        _logger.LogInformation("--------- CatalogIntegrationEventService - Saving changes and integration event: {IntegrationEventId}", integrationEvent.Id);

        await ResilientTransaction.New(_catalogContext).ExecuteAsync(async () =>
        {
            await _catalogContext.SaveChangesAsync();
            await _integrationEventLogService.SaveEventAsync(integrationEvent, _catalogContext.Database.CurrentTransaction);
        });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!dispossedValue)
        {
            if(disposing)
                (_integrationEventLogService as IDisposable)?.Dispose();

            dispossedValue = true;
        }
    }
}
