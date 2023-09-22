namespace EShop.Orders.API.Application.Queries.ViewModels;

public class OrderViewModel
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Description { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string ZipCode { get; init; }
    public string Country { get; init; }

    public List<OrderItemViewModel> OrderItems { get; set; } 
    public decimal Total { get; set; }

    public OrderViewModel()
       => OrderItems = new List<OrderItemViewModel>();
}
