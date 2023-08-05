namespace EShop.Catalog.API.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

    public OrderStatusChangedToPaidIntegrationEventHandler(CatalogDbContext catalogDbContext, 
        ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
    {
        _catalogDbContext = catalogDbContext ?? throw new ArgumentNullException(nameof(catalogDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        using(_logger.BeginScope(new List<KeyValuePair<string, object>> {new ("IntegrationEventContext",@event.Id) })) 
        {
            _logger.LogInformation("Handling Integration Event: {IntegrationEventId} - ({IntegrationEvent})", @event.Id, @event);

            foreach (var orderStockItem in @event.orderStockItems)
            {
                var catalogItem = _catalogDbContext.CatalogItems.Find(orderStockItem.productId);

                if(catalogItem is not null)
                    catalogItem.RemoveStock(orderStockItem.units);
            }

            await _catalogDbContext.SaveChangesAsync();
        }
    }
}