namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrdersByOrderStatus
{
    public int StatusId { get; set; }
    public string StatusName { get; set; }
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string BuyerName { get; set; }
}