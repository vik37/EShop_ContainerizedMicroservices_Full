namespace EShop.Orders.API.Application.Commands;

public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // Simulate a work time for confirming the stock
        await Task.Delay(1000);

        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);

        if(orderToUpdate is null)
            return false;

        orderToUpdate.SetStockConfirmedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class SetStockConfirmedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    public SetStockConfirmedOrderStatusIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand,bool>> logger) : base(mediator, requestManager, logger) { }

    protected override bool CreateResultForDuplicateRequest()
        => true;
}