namespace EShop.Orders.API.Application.IntegrationEvents;

public class OrderIntegrationEventService : IOrderIntegrationEventService
{

    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IEventBus _eventBus;
    private readonly OrderContext _orderContext;
    private readonly IIntegrationEventLogService _integrationEventLogService;
    private readonly ILogger<OrderIntegrationEventService> _logger;

    public OrderIntegrationEventService(Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory, 
        IEventBus eventBus, OrderContext orderContext,
        ILogger<OrderIntegrationEventService> logger)
    {
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
        _integrationEventLogService = integrationEventLogServiceFactory(_orderContext.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent @event)
    {
        _logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", @event.Id, @event);

        await _integrationEventLogService.SaveEventAsync(@event,_orderContext.GetCurrentTransaction());
    }

    public async Task PublicEventsThroughtEventBusAsync(Guid transactionId)
    {
        var pendingLogEvent = await _integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvent in pendingLogEvent)
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvent.EventId, logEvent.IntegrationEvent);

            try
            {
                await _integrationEventLogService.MarkEventAsInProgressAsync(logEvent.EventId);
                _eventBus.Publish(logEvent.IntegrationEvent);
                await _integrationEventLogService.MarkEventAsPublishedAsync(logEvent.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvent.EventId);

                await _integrationEventLogService.MarkEventAsFailedAsync(logEvent.EventId);
            }
        }
    }
}
