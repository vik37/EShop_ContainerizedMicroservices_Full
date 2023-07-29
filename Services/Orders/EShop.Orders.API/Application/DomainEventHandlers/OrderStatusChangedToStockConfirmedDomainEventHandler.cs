namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler : 
    INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{

    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> _logger;

    public OrderStatusChangedToStockConfirmedDomainEventHandler(IOrderRepository orderRepository, 
        IBuyerRepository buyerRepository, ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
        _logger = logger;
    }

    public Task Handle(OrderStatusChangedToStockConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
