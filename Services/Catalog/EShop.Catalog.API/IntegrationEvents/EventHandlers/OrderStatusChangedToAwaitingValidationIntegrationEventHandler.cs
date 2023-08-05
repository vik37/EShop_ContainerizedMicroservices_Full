namespace EShop.Catalog.API.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler
    : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
            CatalogDbContext catalogDbContext, 
            ICatalogIntegrationEventService catalogIntegrationEventService, 
            ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger 
        )
    {
        _catalogDbContext = catalogDbContext ?? throw new ArgumentNullException(nameof(catalogDbContext));
        _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        using(_logger.BeginScope(new List<KeyValuePair<string, object>> {new ("IntegrationEventContext",@event.Id) })) 
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            var confirmedOrderStockïtems = new List<ConfirmedOrderStockItem>();

            foreach (var orderStockItem in @event.orderStockItems)
            {
                var catalogItem = _catalogDbContext.CatalogItems.Find(orderStockItem.productId);
                if(catalogItem is not null)
                {
                    var hasStock = catalogItem.AvailableStock >= orderStockItem.units;
                    var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);
                    confirmedOrderStockïtems.Add(confirmedOrderStockItem);
                }
                
            }

            var confirmedIntegrationEvent = confirmedOrderStockïtems.Any(c => !c.hasStock)
                ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.orderId, confirmedOrderStockïtems)
                : new OrderStockConfirmedIntegrationEvent(@event.orderId);

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
            await _catalogIntegrationEventService.PublishThroughtEventBusAsync(confirmedIntegrationEvent);
        }
    }
}