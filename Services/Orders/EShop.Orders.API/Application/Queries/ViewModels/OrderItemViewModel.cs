namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderItemViewModel
{
    public string ProductName { get; set; } = string.Empty;
    public int Units { get; set; }
    public double UnitPrice { get; set; }
    public string? PictureUrl { get; set; }
}
