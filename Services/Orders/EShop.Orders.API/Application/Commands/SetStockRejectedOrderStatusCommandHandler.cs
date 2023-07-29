namespace EShop.Orders.API.Application.Commands;

public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// Stock service rejects the request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // Simulate a work time for rejecting the stock
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);

        if(orderToUpdate is  null)
            return false;

        orderToUpdate.SetCancelledStatusWhenStockIsRejected(request.OrderStockItems);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

    }
}

public class SetStockRejectedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, bool>
{
    public SetStockRejectedOrderStatusIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand,bool>> logger): base(mediator,requestManager,logger)
    { }

    protected override bool CreateResultForDuplicateRequest()
        => true;
}