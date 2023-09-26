namespace EShop.Web.MVC.Models.ViewModels;

public class OrderSummaryVM
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; } = string.Empty;
    public double Total { get; init; }
}