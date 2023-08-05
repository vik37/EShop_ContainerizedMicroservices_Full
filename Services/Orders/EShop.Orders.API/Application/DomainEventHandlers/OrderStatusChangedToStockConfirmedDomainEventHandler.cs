namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler : 
    INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> _logger;
    private readonly IOrderIntegrationEventService _orderIntegrationEvent;

    public OrderStatusChangedToStockConfirmedDomainEventHandler(IOrderRepository orderRepository,
            IOrderIntegrationEventService orderIntegrationEvent,
            IBuyerRepository buyerRepository, ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger        
        )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderIntegrationEvent = orderIntegrationEvent ?? throw new ArgumentNullException(nameof(orderIntegrationEvent));
    }

    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",notification.OrderId,nameof(OrderStatus.StockConfirmed),OrderStatus.StockConfirmed.Id);

        var order = await _orderRepository.GetAsync(notification.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId!.Value.ToString());

        var integrationEvent = new OrderStatusChangedToStockConfirmedIntegrationEvent(order.Id, order.OrderStatus!.Name, buyer.Name!);
        await _orderIntegrationEvent.AddAndSaveEventAsync(integrationEvent);
    }
}
