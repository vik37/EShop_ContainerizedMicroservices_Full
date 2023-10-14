namespace EShop.Orders.API.Application.Queries.OrderQueries.ViewModels;

public class OrderSummaryViewModel
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; } = string.Empty;
    public double Total { get; init; }
}
