namespace EShop.Orders.BackgroundTask.Events;

public record GracePeriodConfirmedIntegrationEvent(int orderId) : IntegrationEvent;