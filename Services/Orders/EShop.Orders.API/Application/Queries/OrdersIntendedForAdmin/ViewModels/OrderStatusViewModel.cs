namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class OrderStatusViewModel
{
    public int Id { get; init; }
    public string Name { get; init; }

    public OrderStatusViewModel(int id, string name)
    {
        Id = id;
        Name = name.EditOrderStatusName();
    }
}