namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(int orderId, string orderStatus, string buyerName,
                                                        IEnumerable<OrderStockItem> orderStockItems)
    : IntegrationEvent;
