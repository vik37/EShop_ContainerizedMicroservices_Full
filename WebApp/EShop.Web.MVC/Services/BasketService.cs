namespace EShop.Web.MVC.Services;

public class BasketService : BaseService, IBasketService
{
    public BasketService(IHttpClientFactory httpClientFactory) 
        : base(httpClientFactory)
    { }

    public async Task<Basket> GetBasket()
    {
        var http = _httpClientFactory.CreateClient("BasketAPI");
        string path = BasketAPI.GetBasketByUserIdURIPath();
        HttpResponseMessage httpResponseMessage = await http.GetAsync(path);
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        var basket = JsonConvert.DeserializeObject<Basket>(content) ?? new Basket();
        http.DefaultRequestHeaders.Clear();
        return basket;
    }

    public async Task<Basket> UpdateBasket(Basket basket)
    {
        var http = _httpClientFactory.CreateClient("BasketAPI");
        var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");
        var response = await http.PostAsync(string.Empty, basketContent);

        response.EnsureSuccessStatusCode();
        http.DefaultRequestHeaders.Clear();
        return basket;
    }
}
