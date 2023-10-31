namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrdersByOrderStatus
{
    private string _status;

    public int StatusId { get; set; }

    public string StatusName
    {
        get => this._status;
        set { this._status = value.EditOrderStatusName(); }
    }

    public int OrderNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public string BuyerName { get; set; }
    public string PaidBy { get; set; }

    public long QuantityByDifferentProduct { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalPrice { get; set; }
}