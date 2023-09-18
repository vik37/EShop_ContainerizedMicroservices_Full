namespace EShop.Web.MVC.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetMyOrders();
    Task<Order> GetOrder(string orderId);
    Task<bool> Create(OrderCheckoutDto order);
    Task CancelOrder(string orderId);
    Task ShipOrder(string orderId);

    OrderCheckoutDto MapOrderToCheckout(Order order);
}