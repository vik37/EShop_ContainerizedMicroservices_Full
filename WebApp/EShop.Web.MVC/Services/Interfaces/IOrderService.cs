namespace EShop.Web.MVC.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderSummaryVM>> GetOrderSummaryByUser(string userId);
    Task<OrderVM> GetOrder(string userId,int orderId);
    Task<List<OrderItemVM>> GetAllOrderedProductsByUser(string userId);
    Task<bool> Create(OrderCheckoutDto order);
    Task CancelOrder(string orderId);
    Task ShipOrder(string orderId);

    OrderCheckoutDto MapOrderToCheckout(Order order);
}