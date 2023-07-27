namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record GracedPeriodConfirmedIntegrationEvent(int orderId) : IntegrationEvent;

