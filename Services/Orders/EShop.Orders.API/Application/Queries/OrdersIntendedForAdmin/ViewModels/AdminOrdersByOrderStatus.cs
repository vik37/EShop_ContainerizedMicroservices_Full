namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrdersByOrderStatus
{
    private string _status;

    public int OrderNumber { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }

    public string Status
    {
        get => this._status;
        set { this._status = value.EditOrderStatusName(); }
    }

    public DateTime OrderDate { get; set; }
    public string BuyerName { get; set; }
}