namespace Web.BFF.ShoppingAggregator.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(HttpClient httpClient, IOptions<APIUrlsOptionSettings> optionSettings):
        base(httpClient, optionSettings) { }

    public async Task<OrderData?> GetOrderDraftAsync(BasketData basketData)
    {
        string url = $"{_optionSettings.OrderAPIDocker}{OrdersAPIUrlPaths.GetOrderDraft}";

        var content = new StringContent(JsonConvert.SerializeObject(basketData), System.Text.Encoding.UTF8,"application/json");

        var response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();

        var orderDraftResponse = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<OrderData>(orderDraftResponse)??null;
    }
}