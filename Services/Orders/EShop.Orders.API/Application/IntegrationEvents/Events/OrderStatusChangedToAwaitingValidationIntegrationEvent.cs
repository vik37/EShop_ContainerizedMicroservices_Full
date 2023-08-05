namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(int orderId, string orderStatus, string buyerName, 
                                                                IEnumerable<OrderStockItem> orderStockItems)
    : IntegrationEvent;

public record OrderStockItem(int productId, int unit);
