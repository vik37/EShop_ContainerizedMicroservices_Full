namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderSummaryViewModel
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; } = string.Empty;
    public double Total { get; init; }
}
