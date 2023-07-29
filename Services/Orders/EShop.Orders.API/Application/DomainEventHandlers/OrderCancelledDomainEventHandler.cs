namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<OrderCancelledDomainEventHandler> _logger;
    //private readonly IOrderIntegrationEventService _integrationEventService;

    public OrderCancelledDomainEventHandler(IOrderRepository orderRepository, IBuyerRepository buyerRepository, 
        ILogger<OrderCancelledDomainEventHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //_integrationEventService = integrationEventService;
    }

    public async Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",notification.Order.Id,nameof(OrderStatus.Cancelled),OrderStatus.Cancelled.Id);

        var order = await _orderRepository.GetAsync(notification.Order.Id);

        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value.ToString());
    }
}
