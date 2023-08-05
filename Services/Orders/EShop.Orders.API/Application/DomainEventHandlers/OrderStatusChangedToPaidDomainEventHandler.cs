namespace EShop.Orders.API.Application.DomainEventHandlers;

public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<OrderStatusChangedToPaidDomainEventHandler> _logger;
    private readonly IOrderIntegrationEventService _orderIntegrationEventService;

    public OrderStatusChangedToPaidDomainEventHandler(IOrderRepository orderRepository, 
        IBuyerRepository buyerRepository, ILogger<OrderStatusChangedToPaidDomainEventHandler> logger, 
        IOrderIntegrationEventService orderIntegrationEventService
        )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderIntegrationEventService = orderIntegrationEventService ?? throw new ArgumentNullException(nameof(orderIntegrationEventService));
    }

    public async Task Handle(OrderStatusChangedToPaidDomainEvent orderStatusChangedDE, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})", orderStatusChangedDE.OrderId,nameof(OrderStatus.Paid),OrderStatus.Paid.Id);

        var order = await _orderRepository.GetAsync(orderStatusChangedDE.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId!.Value.ToString());

        var orderStockList = orderStatusChangedDE.OrderItems.Select(x => new OrderStockItem(x.ProductId, x.GetUnits()));

        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(orderStatusChangedDE.OrderId, order.OrderStatus!.Name,
                                                                                buyer.Name!, orderStockList);

        await _orderIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}