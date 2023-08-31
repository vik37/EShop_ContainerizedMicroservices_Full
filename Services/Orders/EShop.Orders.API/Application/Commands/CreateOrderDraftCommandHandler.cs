namespace EShop.Orders.API.Application.Commands;

public class CreateOrderDraftCommandHandler : IRequestHandler<CreateOrderDraftCommand, OrderDraftDto>
{
    public Task<OrderDraftDto> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
    {
        Order order = Order.NewDraft();
        var orderItems = request.Items.Select(x => x.ToOrderItemDto()).ToList();

        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl);
        }
        return Task.FromResult(OrderDraftDto.FromOrder(order));
    }
}
