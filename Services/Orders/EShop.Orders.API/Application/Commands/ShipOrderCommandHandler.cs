namespace EShop.Orders.API.Application.Commands;

public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public ShipOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// administrator executes ship order from app
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);

        if(orderToUpdate is null)
            return false;

        orderToUpdate.SetShippedStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class ShipOrderIdentifiedCommandHandler : IdentifiedCommandHandler<ShipOrderCommand, bool>
{
    public ShipOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<ShipOrderCommand,bool>> logger)
        : base(mediator, requestManager, logger) { }

    protected override bool CreateResultForDuplicateRequest()
        => true;
}