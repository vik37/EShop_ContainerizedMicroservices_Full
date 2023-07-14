namespace EShop.Orders.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvents>
{
    public Task Handle(OrderStartedDomainEvents notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
