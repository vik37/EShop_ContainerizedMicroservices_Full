namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrderSummaryViewModel
{
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string BuyerName { get; set; }
    public double Total { get; set; }
}