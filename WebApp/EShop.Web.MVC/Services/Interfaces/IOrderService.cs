namespace EShop.Web.MVC.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderSummaryVM>> GetMyOrderSummary(string userId);
    Task<OrderVM> GetOrder(int orderId);
    Task<bool> Create(OrderCheckoutDto order);
    Task CancelOrder(string orderId);
    Task ShipOrder(string orderId);

    OrderCheckoutDto MapOrderToCheckout(Order order);
}