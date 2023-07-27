namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent(int orderId) : IntegrationEvent;