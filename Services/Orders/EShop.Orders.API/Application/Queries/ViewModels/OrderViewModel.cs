namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderViewModel
{
    public int OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }

    public List<OrderItemViewModel> OrderItems { get; set; } 
    public decimal Total { get; set; }

    public OrderViewModel()
       => OrderItems = new List<OrderItemViewModel>();
}
