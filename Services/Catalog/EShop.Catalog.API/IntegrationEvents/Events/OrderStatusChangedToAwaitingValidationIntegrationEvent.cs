namespace EShop.Catalog.API.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(int orderId, IEnumerable<OrderStockItem> orderStockItems)
    : IntegrationEvent;

public record OrderStockItem(int productId, int units);