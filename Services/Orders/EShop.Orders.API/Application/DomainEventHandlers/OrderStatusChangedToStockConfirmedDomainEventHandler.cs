namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler : 
    INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{

    private readonly OrderRepository _orderRepository;
    private readonly BuyerRepository _buyerRepository;
    private readonly ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> _logger;

    public OrderStatusChangedToStockConfirmedDomainEventHandler(OrderRepository orderRepository, 
        BuyerRepository buyerRepository, ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger)
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
