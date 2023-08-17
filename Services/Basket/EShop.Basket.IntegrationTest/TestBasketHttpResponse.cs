namespace Basket.IntegrationTest;

public class TestBasketHttpResponse : IClassFixture<BasketWebApplicationFactory>
{
    private readonly BasketWebApplicationFactory _applicationFactory;
    private readonly HttpClient _httpClient;
    private string userId = "a6fb58a6-6afb-4a37-9a98-79329e4977d3";

    public TestBasketHttpResponse(BasketWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _httpClient = applicationFactory.CreateClient();
    }

    [Fact]
    public async Task Test_GetProductFromBasketByUserId_ResponseStatusShouldBeOk()
    {
        var response = await _httpClient.GetAsync(BasketURIPath.GetProductFromBasketByUserIdURIPath(userId));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Test_AddNewProductToBasketByUserId_ResponseStatusShouldBeOk()
    {
        var customerBasket = new CustomerBasket
        {
            BuyerId = userId,
            Items = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = 1,
                    ProductId = 1,
                    ProductName = "Test Name",
                    Quantity = 1,
                    PictureUrl= "",
                    UnitPrice = 15.24m,
                    OldUnitPrice = 0
                }
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(customerBasket), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BasketURIPath.AddNewProductToBasket, content);

        response.EnsureSuccessStatusCode();
    }
}
