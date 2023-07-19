namespace EShop.Orders.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvents>
{
    private readonly ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> _logger;
    private readonly IBuyerRepository _buyerRepository;
    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger, 
                                                                            IBuyerRepository buyerRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
    }

    public async Task Handle(OrderStartedDomainEvents orderStartedEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;

        // JUST FOR TESTING BECAUSE THE USER IDENTITY SERVICE IS NOT DONE YET!
        var userId = "0755503e-dac3-4980-a96a-41178d982380";

        var buyer = await _buyerRepository.FindAsync(userId);

        bool buyerOriginallyExisted = (buyer is null) ?  false : true;

        if (buyerOriginallyExisted)
            buyer = new Buyer(orderStartedEvent.UserId,orderStartedEvent.UserName);

        buyer!.VerifyOrAddPayment(cardTypeId, $"Payment Method on: {DateTime.UtcNow}",orderStartedEvent.CardNumber,orderStartedEvent.CardSecurityNumber,
                                    orderStartedEvent.CardHolderName,orderStartedEvent.CardExpiration,orderStartedEvent.Order.Id);

        var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.AddBuyer(buyer);

        await _buyerRepository.UnitOfWork.SaveChangesAsync();

        _logger.LogTrace("Buyer {BuyerId} and related payment method were validated or updated for order Id: {OrderId}.",buyerUpdated.Id,orderStartedEvent.Order.Id);
    }
}
