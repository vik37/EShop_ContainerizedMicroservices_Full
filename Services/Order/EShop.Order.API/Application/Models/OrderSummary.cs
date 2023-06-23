namespace EShop.Order.API.Application.Models;

public class OrderSummary
{
    public int OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public double Total { get; set; }
}
