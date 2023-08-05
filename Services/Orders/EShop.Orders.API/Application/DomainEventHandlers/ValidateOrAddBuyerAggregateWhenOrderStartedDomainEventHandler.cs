namespace EShop.Orders.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvents>
{
    private readonly ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
        ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger, 
        IBuyerRepository buyerRepository,
        IOrderIntegrationEventService orderIntegrationEventService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _orderIntegrationEventService = orderIntegrationEventService ?? throw new ArgumentNullException(nameof(orderIntegrationEventService));
    }

    public async Task Handle(OrderStartedDomainEvents orderStartedEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;

        var buyer = await _buyerRepository.FindAsync(orderStartedEvent.UserId);

        bool buyerOriginallyExisted = (buyer is null) ?  false : true;

        if (buyerOriginallyExisted)
            buyer = new Buyer(orderStartedEvent.UserId,orderStartedEvent.UserName);

        buyer!.VerifyOrAddPayment(cardTypeId, $"Payment Method on: {DateTime.UtcNow}",orderStartedEvent.CardNumber,orderStartedEvent.CardSecurityNumber,
                                    orderStartedEvent.CardHolderName,orderStartedEvent.CardExpiration,orderStartedEvent.Order.Id);

        var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.AddBuyer(buyer);

        await _buyerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmitedIntegrationEvent(orderStartedEvent.Order.Id,
            orderStartedEvent.Order.OrderStatus!.Name, buyer.Name!);
        await _orderIntegrationEventService.AddAndSaveEventAsync(integrationEvent);

        _logger.LogTrace("Buyer {BuyerId} and related payment method were validated or updated for order Id: {OrderId}.",buyerUpdated.Id,orderStartedEvent.Order.Id);
    }
}
