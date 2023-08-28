namespace EShop.Web.MVC.Services;

public class BasketService : BaseService, IBasketService
{
    public BasketService(HttpClient httpClient, Retry retry) 
        : base(httpClient, retry)
    { }

    public async Task<Basket> GetBasket()
    {
        string uriPath = BasketAPI.GetBasketByUserIdURIPath();
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => _httpClient.GetAsync(uriPath));
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var basket = JsonConvert.DeserializeObject<Basket>(content) ?? new Basket();
        _httpClient.DefaultRequestHeaders.Clear();
        return basket;
    }

    public async Task<Basket> UpdateBasket(Basket basket)
    {
        var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("basket", basketContent);

        response.EnsureSuccessStatusCode();
        _httpClient.DefaultRequestHeaders.Clear();
        return basket;
    }

    public async Task RemoveAllItems(string userId)
    {
        var response = await _httpClient.DeleteAsync("basket/"+userId);
        _httpClient.DefaultRequestHeaders.Clear();
        response.EnsureSuccessStatusCode();
    }

    public async Task Checkout(BasketDTO basket)
    {
        var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(BasketAPI.BasketCheckoutURIPath, basketContent);

        response.EnsureSuccessStatusCode();
        _httpClient.DefaultRequestHeaders.Clear();
    }
}
