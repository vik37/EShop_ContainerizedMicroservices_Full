namespace EShop.Web.MVC.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetMyOrders();
    Task<Order> GetOrder(string orderId);
    Task CancelOrder(string orderId);
    Task ShipOrder(string orderId);
    BasketDTO MapOrderToBasket(Order order);
    void OverrideUserInfoIntoOrder(Order original, Order destination);
    Order MapUserInfoIntoOrder(ApplicationUser applicationUser, Order order);
}