namespace EShop.Orders.API.Application.Commands;

public class SetStockRejectedOrderStatusCommand : IRequest<bool>
{
    [DataMember]
    public int OrderNumber { get; set; }

    [DataMember]
    public IEnumerable<int> OrderStockItems { get; set; }

    public SetStockRejectedOrderStatusCommand(int orderNumber, IEnumerable<int> orderStockItems)
    {
        OrderNumber = orderNumber;
        OrderStockItems = orderStockItems;
    }
}