namespace EShop.Basket.API.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent(string userId) : IntegrationEvent;
