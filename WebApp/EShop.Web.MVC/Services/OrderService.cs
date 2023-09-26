namespace EShop.Web.MVC.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(HttpClient httpClient, Retry retry) 
        : base(httpClient, retry) { }

    public async Task<OrderVM> GetOrder(string userId, int orderId)
    {
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(OrderAPI.GetOrderById(userId,orderId)));
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<OrderVM>(content);

        _httpClient.DefaultRequestHeaders.Clear();
        return order;
    }

    public async Task<List<OrderSummaryVM>> GetMyOrderSummary(string userId)
    {
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(OrderAPI.GetOrdersByUserId(userId)));
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var orderSummary = JsonConvert.DeserializeObject<IEnumerable<OrderSummaryVM>>(content);
        _httpClient.DefaultRequestHeaders.Clear();
        return orderSummary.ToList();
    }

    public async Task<bool> Create(OrderCheckoutDto order)
    {
        var basketContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

        var response = await _policy.ExecuteAsync(() => _httpClient.PostAsync(OrderAPI.Create, basketContent));

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
            return false;

        _httpClient.DefaultRequestHeaders.Clear();
        return true;
    }

    public Task CancelOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task ShipOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public OrderCheckoutDto MapOrderToCheckout(Order order)
    {
        order.CardExpirationAPIFormat();

        return new OrderCheckoutDto
        {
            UserId = order.Buyer,
            UserName = order.Username,
            City = order.City,
            Street = order.Street,
            State = order.State,
            Country = order.Country,
            ZipCode = order.ZipCode,
            CardNumber = order.CardNumber,
            CardHolderName = order.CardHolderName,
            CardExpiration = order.CardExpirationDate,
            CardSecurityNumber = order.CardSecurityNumber,
            CardTypeId = 1,
            OrderItems = order.OrderItems 
        };
    }   
}