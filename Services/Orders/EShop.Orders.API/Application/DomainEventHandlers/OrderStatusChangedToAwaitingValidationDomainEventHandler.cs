namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler
    : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public OrderStatusChangedToAwaitingValidationDomainEventHandler(IOrderRepository orderRepository, 
            IBuyerRepository buyerRepository,
            IOrderIntegrationEventService orderIntegrationEventService, 
            ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger
        )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        //_orderIntegrationEventService = orderIntegrationEventService ?? throw new ArgumentNullException(nameof(orderIntegrationEventService));
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})", 
            notification.OrderId, nameof(OrderStatus.AwaitingValidation),OrderStatus.AwaitingValidation.Id);

        var order = await _orderRepository.GetAsync(notification.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId!.Value.ToString());

        var orderStockList = notification.OrderItems.Select(x => new OrderStockItem(x.ProductId, x.GetUnits()));

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(order.Id, order.OrderStatus!.Name, buyer.Name!, orderStockList);
        await _orderIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
