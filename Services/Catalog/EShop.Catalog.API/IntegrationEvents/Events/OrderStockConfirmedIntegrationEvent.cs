namespace EShop.Catalog.API.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent(int orderId) : IntegrationEvent;