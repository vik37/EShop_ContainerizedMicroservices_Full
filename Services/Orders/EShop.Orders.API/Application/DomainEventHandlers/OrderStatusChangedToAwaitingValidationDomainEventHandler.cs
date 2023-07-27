namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler
    : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public OrderStatusChangedToAwaitingValidationDomainEventHandler(IOrderRepository orderRepository, 
        ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger, IBuyerRepository buyerRepository, 
        IOrderIntegrationEventService orderIntegrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository;
        _orderIntegrationEventService = orderIntegrationEventService;
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})", OrderStatus.AwaitingValidation.Id);

        var order = await _orderRepository.GetAsync(notification.OrderId);
        //var buyer = await _buyerRepository.FindByIdAsync(order.Ge)
    }
}
