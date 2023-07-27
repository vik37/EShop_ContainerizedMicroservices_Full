namespace EShop.Orders.API.Application.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent(string userId) : IntegrationEvent;
