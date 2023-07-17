namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderItemViewModel
{
    public string ProductName { get; init; } = string.Empty;
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string? PictureUrl { get; init; }
}
