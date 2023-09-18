namespace EShop.Orders.API.Application.Commands;

public class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// Shipment service confirms the payment
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetPaidOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // Simulate a work time for validating the payment
        await Task.Delay(1000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(request.OrderNumber);

        if (orderToUpdate is null)
            return false;

        orderToUpdate.SetPaiedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
