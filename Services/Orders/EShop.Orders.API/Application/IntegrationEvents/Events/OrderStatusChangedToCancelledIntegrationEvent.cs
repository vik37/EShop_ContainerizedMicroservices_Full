namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToCancelledIntegrationEvent(int orderId, string orderStatus, string buyerName)
    : IntegrationEvent;
