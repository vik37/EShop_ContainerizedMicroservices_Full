namespace EShop.Orders.API.Application.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : 
        INotificationHandler<BuyerAndPeymentMethodVerifiedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> _logger;

    public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository,
        ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  When the Buyer and Buyer's payment method have been created or verified that they existed, 
    ///  then we can update the original Order with the BuyerId and PaymentId (foreign keys)
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(BuyerAndPeymentMethodVerifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(domainEvent.OrderId);
        orderToUpdate.SetBuyerId(domainEvent.Buyer.Id);
        orderToUpdate.SetPaymentId(domainEvent.PaymentMethod.Id);

        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})",domainEvent.OrderId,nameof(domainEvent.PaymentMethod) ,domainEvent.PaymentMethod.Id);
    }
}
