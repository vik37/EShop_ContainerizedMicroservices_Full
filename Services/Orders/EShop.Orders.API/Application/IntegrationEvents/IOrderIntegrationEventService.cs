namespace EShop.Orders.API.Application.IntegrationEvents;

public interface IOrderIntegrationEventService
{
    Task PublicEventsThroughtEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent @event);
}
