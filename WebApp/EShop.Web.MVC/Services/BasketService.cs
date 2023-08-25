namespace EShop.Web.MVC.Services;

public class BasketService : BaseService, IBasketService
{
    public BasketService(IHttpClientFactory httpClientFactory, Retry retry) 
        : base(httpClientFactory, retry)
    { }

    public async Task<Basket> GetBasket()
    {
        var http = _httpClientFactory.CreateClient("BasketAPI");
        string uriPath = BasketAPI.GetBasketByUserIdURIPath();
        HttpResponseMessage httpResponseMessage = await _policy.ExecuteAsync(() => http.GetAsync(uriPath));
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var basket = JsonConvert.DeserializeObject<Basket>(content) ?? new Basket();
        http.DefaultRequestHeaders.Clear();
        return basket;
    }

    public async Task<Basket> UpdateBasket(Basket basket)
    {
        var http = _httpClientFactory.CreateClient("BasketAPI");
        var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");
        var response = await http.PostAsync("basket", basketContent);

        response.EnsureSuccessStatusCode();
        http.DefaultRequestHeaders.Clear();
        return basket;
    }

    public async Task RemoveAllItems(string userId)
    {
        var http = _httpClientFactory.CreateClient("BasketAPI");

        var response = await http.DeleteAsync("basket/"+userId);
        response.EnsureSuccessStatusCode();
    }
}
