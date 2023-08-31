namespace Web.BFF.ShoppingAggregator.Services;

public class BasketService : BaseService, IBasketService
{
    public BasketService(HttpClient httpClient, IOptions<APIUrlsOptionSettings> optionsSettongs)
        :base(httpClient, optionsSettongs) { }
    
    public async Task<BasketData?> GetBasketByIdAsync(string id)
    {
        string url = $"{_optionSettings.BasketAPIDocker}{BasketAPIUrlPaths.GetBasketByUserIdAsync(id)}";

        var content = await _httpClient.GetStringAsync(url);

        var basketData = JsonConvert.DeserializeObject<BasketData>(content);

        return basketData;
    }
}