namespace EShop.Orders.API.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;
    // private readonly IOrderIntegrationEventService _integrationEventService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator, 
         ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        // _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Add Integration event to clean the basket
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);

       // await _integrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

        // Add/Update the Buyer AggregateRoot
        // DDD patterns comment: Add child entities and value-objects through the Order Aggregate - Root
        // methods and constructor so validations, invariants and business logic
        // make sure that consistency is preserved across the whole aggregate

        var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);

        var order = new Order(request.UserId, request.UserName ,address,request.CardTypeId,request.CardNumber,request.CardSecurityNumber,
                                request.CardHolderName,request.CardExpiration);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        _logger.LogInformation("------ Creating Order - Order: {@Order}",  order);

        _orderRepository.Add(order);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
{
    public CreateOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CreateOrderCommand,bool>> logger) : base(mediator, requestManager, logger) { }

    protected override bool CreateResultForDuplicateRequest()
        => true; // Ignore duplicate requests for processing order.
}