namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToShippedIntegrationEvent(int orderId, string orderStatus, string buyerName) : IntegrationEvent;
