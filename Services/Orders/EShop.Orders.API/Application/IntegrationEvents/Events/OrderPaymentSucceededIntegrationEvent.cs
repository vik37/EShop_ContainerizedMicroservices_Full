namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent(int orderId) : IntegrationEvent;
