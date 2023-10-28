namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrderSummaryViewModel
{
    private string _status;

    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string Description { get; set; }

    public string Status
    {
        get => this._status;
        set { this._status = value.EditOrderStatusName(); }
    }

    public string BuyerName { get; set; }
    public double Total { get; set; }
}