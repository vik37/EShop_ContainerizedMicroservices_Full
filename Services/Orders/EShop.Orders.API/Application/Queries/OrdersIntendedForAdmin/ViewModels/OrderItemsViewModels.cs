namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class OrderItemsViewModels
{
    public string ProductName { get; init; }
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string PictureUrl { get; init; }
}