namespace EShop.Orders.BackgroundTask.Services;

public class GracePeriodManagerService : BackgroundService
{
    private readonly ILogger<GracePeriodManagerService> _logger;
    private readonly IEventBus _eventBus;
    private readonly BackgroundTaskOptionSettings _optionSettings;

    public GracePeriodManagerService(ILogger<GracePeriodManagerService> logger, 
        IEventBus eventBus, 
        IOptions<BackgroundTaskOptionSettings> optionSettings)
    {
        _logger = logger;
        _eventBus = eventBus;
        _optionSettings = optionSettings.Value;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("GracePeriodManagerService is starting.");

        stoppingToken.Register(() => _logger.LogDebug("#1 Background Task is Stopping. "));

        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Background task is running!!! ");

            CheckConfirmedGracePeriodOrders();
            await Task.Delay(_optionSettings.CheckUpdateTime, stoppingToken);
        }
    }

    private void CheckConfirmedGracePeriodOrders()
    {
        _logger.LogDebug("Check confirmed grace period orders");

        var orderIds = GetConfirmedGracePeriodOrders();

        foreach (var orderId in orderIds)
        {
            var confirmedGracePeriodEvent = new GracePeriodConfirmedIntegrationEvent(orderId);

            _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", confirmedGracePeriodEvent.Id, confirmedGracePeriodEvent);
            _eventBus.Publish(confirmedGracePeriodEvent);
        }
    }

    private IEnumerable<int> GetConfirmedGracePeriodOrders()
    {
        IEnumerable<int> orderIds = new List<int>();

        using var connection = new SqlConnection(_optionSettings.DbContext);
        try
        {
            connection.Open();
            orderIds = connection.Query<int>(
                    @"SELECT Id FROM [ordering].[Orders]
                    WHERE DATEDIFF(minute, [OrderDate], GETDATE()) > @GracePeriodTime
                    AND [OrderStatusId] = 1", new { _optionSettings.GracePeriodTime }
                );
        }
        catch (SqlException ex)
        {
            _logger.LogCritical(ex, "Fatal Error establishing database connection");
        }
        return orderIds;
    }
}