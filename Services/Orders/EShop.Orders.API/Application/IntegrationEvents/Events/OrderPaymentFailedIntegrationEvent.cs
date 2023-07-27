namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent(int orderId) : IntegrationEvent;
