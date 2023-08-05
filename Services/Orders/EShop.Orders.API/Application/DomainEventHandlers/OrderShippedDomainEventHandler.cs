namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderShippedDomainEventHandler : INotificationHandler<OrderShippedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<OrderShippedDomainEventHandler> _logger;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public OrderShippedDomainEventHandler(IOrderRepository orderRepository, 
        IBuyerRepository buyerRepository, 
        IOrderIntegrationEventService orderIntegrationEventService,
        ILogger<OrderShippedDomainEventHandler> logger
        )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderIntegrationEventService = orderIntegrationEventService ?? throw new ArgumentNullException(nameof(orderIntegrationEventService));
    }

    public async Task Handle(OrderShippedDomainEvent orderShippedDomainEvent, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})", orderShippedDomainEvent.Order.Id, nameof(orderShippedDomainEvent.Order.OrderStatus.Name), orderShippedDomainEvent.Order.OrderStatus!.Id);

        var order = await _orderRepository.GetAsync(orderShippedDomainEvent.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId!.Value.ToString());

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(order.Id, order.OrderStatus!.Name, buyer.Name!);
        await _orderIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}