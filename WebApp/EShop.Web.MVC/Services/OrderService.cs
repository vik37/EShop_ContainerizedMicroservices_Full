namespace EShop.Web.MVC.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(HttpClient httpClient, Retry retry) 
        : base(httpClient, retry) { }

    public Task CancelOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetMyOrders()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public BasketDTO MapOrderToBasket(Order order)
    {
        throw new NotImplementedException();
    }

    public void OverrideUserInfoIntoOrder(Order origin, Order destination)
    {
        throw new NotImplementedException();
    }

    public Task ShipOrder(string orderId)
    {
        throw new NotImplementedException();
    }
}