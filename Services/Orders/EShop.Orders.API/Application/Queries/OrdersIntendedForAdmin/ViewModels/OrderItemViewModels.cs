namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class OrderItemViewModels
{
    public string ProductName { get; init; }
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string PictureUrl { get; init; }
}