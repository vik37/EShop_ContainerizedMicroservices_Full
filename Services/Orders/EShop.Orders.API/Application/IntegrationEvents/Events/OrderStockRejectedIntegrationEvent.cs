namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(int orderId, List<ConfirmedOrderStockItem> orderStockItems) 
    : IntegrationEvent;

public record ConfirmedOrderStockItem(int productId, bool hasStock);
