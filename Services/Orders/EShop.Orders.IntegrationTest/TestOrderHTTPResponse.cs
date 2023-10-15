namespace EShop.Orders.IntegrationTest;

public class TestOrderHTTPResponse  : IClassFixture<OrderWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly OrderWebApplicationFactory _orderWebApplicationFactory;

    public TestOrderHTTPResponse(OrderWebApplicationFactory orderWebApplicationFactory)
    {
        _orderWebApplicationFactory = orderWebApplicationFactory;
        _httpClient = _orderWebApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Test_GetCardTypes_EnshureStatusCodeOk()
    {
        var response = await _httpClient.GetAsync(OrderURIPath.GetAllCardTypes);

        var content = await response.Content.ReadAsStringAsync();

        var cardTypesResponse = JsonConvert.DeserializeObject<IEnumerable<CardType>>(content);

        var amex = cardTypesResponse!.Single(x => x.Id == 1);

        CardType amexCardType = CardType.Amex;
        response.EnsureSuccessStatusCode();
        Assert.Equal(amexCardType.Name, amex.Name);
        Assert.Equal(amexCardType.Id, amex.Id);
        Assert.True(cardTypesResponse!.Count() > 0);
        Assert.NotNull(cardTypesResponse);
        Assert.Contains("Visa", cardTypesResponse.Single(x => x.Id == 2).Name);
    }

    [Fact]
    public async Task Test_CancleOrder_Failed_ShouldReturnBadRequest()
    {
        var content = new StringContent(ShippedOrderBuilder(), UTF8Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestId", Guid.NewGuid().ToString() } }
        };

        var response = await _httpClient.PutAsync(OrderURIPath.CancelOrder, content);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Test_GetOrderById_Failes_ShouldReturnNotFound()
    {

        var response = await _httpClient.GetAsync(OrderURIPath.GetOrderById(1));

        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }

    [Fact]
    public async Task Test_ShipOrder_Failed_ShouldReturnBadRequest()
    {
        var content = new StringContent(ShippedOrderBuilder(), UTF8Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestId", Guid.NewGuid().ToString() } }
        };

        var response = await _httpClient.PutAsync(OrderURIPath.ShipOrder, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private string ShippedOrderBuilder()
    {
        var order = new ShipOrderCommand(1);
        return JsonConvert.SerializeObject(order);
    }
}