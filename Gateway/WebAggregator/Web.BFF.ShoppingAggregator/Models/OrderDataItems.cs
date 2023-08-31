namespace Web.BFF.ShoppingAggregator.Models;

public class OrderDataItems
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; init; }
    public int Units { get; init; }
    public string PictureUrl { get; init; } = string.Empty;
}