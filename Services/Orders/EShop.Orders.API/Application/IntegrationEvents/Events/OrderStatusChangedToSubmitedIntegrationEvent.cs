namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToSubmitedIntegrationEvent(int orderId, string orderStatus, string buyerName)
    : IntegrationEvent;
