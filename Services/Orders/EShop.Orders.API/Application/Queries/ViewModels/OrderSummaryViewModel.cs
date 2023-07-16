namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderSummaryViewModel
{
    public int OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public double Total { get; set; }
}
