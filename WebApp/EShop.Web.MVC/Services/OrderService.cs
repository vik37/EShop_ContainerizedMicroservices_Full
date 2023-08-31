namespace EShop.Web.MVC.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(HttpClient httpClient, Retry retry) 
        : base(httpClient, retry) { }

    public Task<Order> GetOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetMyOrders()
    {
        throw new NotImplementedException();
    }

    

    public Task CancelOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task ShipOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public BasketDTO MapOrderToBasket(Order order)
    {
        order.CardExpirationAPIFormat();

        return new BasketDTO
        {
            City = order.City,
            Street = order.Street,
            State = order.State,
            Country = order.Country,
            ZipCode = order.ZipCode,
            CardNumber = order.CardNumber,
            CardHolderName = order.CardHolderName,
            CardExpirationDate = order.CardExpirationDate,
            CardSecurityNumber = order.CardSecurityNumber,
            CardTypeId = 1,
            Buyer = order.Buyer,
            RequestId = order.RequestId.ToString()
        };
    }

    public void OverrideUserInfoIntoOrder(Order original, Order destination)
    {
        destination.City = original.City;
        destination.Street = original.Street;
        destination.State = original.State;
        destination.Country = original.Country;
        destination.ZipCode = original.ZipCode;

        destination.CardNumber = original.CardNumber;
        destination.CardHolderName = original.CardHolderName;
        destination.CardExpirationDate = original.CardExpirationDate;
        destination.CardSecurityNumber = original.CardSecurityNumber;
    }

    public Order MapUserInfoIntoOrder(ApplicationUser applicationUser, Order order)
    {
        order.City = applicationUser.City;
        order.Street = applicationUser.Street;
        order.State = applicationUser.State;
        order.Country = applicationUser.Country;
        order.ZipCode = applicationUser.ZipCode;
        order.CardNumber = applicationUser.CardNumber;
        order.CardHolderName = applicationUser.CardHolderName;
        order.CardExpirationDate = new DateTime(int.Parse(string.Concat("20", applicationUser.Expiration.Split('/')[1])), int.Parse(applicationUser.Expiration.Split('/')[0]), 1);
        order.CardSecurityNumber = applicationUser.SecurityNumber;
        return order;
    }
}