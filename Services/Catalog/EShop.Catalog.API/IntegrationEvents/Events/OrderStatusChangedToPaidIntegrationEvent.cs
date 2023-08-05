namespace EShop.Catalog.API.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(int orderId, IEnumerable<OrderStockItem> orderStockItems)
    : IntegrationEvent;
