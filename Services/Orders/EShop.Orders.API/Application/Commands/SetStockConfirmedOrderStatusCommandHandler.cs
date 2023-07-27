namespace EShop.Orders.API.Application.Commands;

public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    private readonly OrderRepository _orderRepository;

    public SetStockConfirmedOrderStatusCommandHandler(OrderRepository orderRepository)
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
