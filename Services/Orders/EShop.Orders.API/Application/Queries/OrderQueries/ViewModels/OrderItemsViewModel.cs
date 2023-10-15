namespace EShop.Orders.API.Application.Queries.OrderQueries.ViewModels;

public class OrderItemsViewModel
{
    public string ProductName { get; init; }
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string PictureUrl { get; init; }
}
