using EShop.Orders.Domain.AggregatesModel.BuyerAggregate;

namespace EShop.Orders.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvents>
{
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(ILoggerFactory logger, IBuyerRepository buyerRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
    }

    public Task Handle(OrderStartedDomainEvents notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
