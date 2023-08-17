namespace Basket.IntegrationTest.TestingBasketRepositories;

public class Test_BasketRepo : IClassFixture<BasketWebApplicationFactory>
{
    private readonly BasketWebApplicationFactory _applicationFactory;
    private readonly IBasketRepository _basketRepository;
    private string userId = "539dd118-23d6-4196-8ead-b13747cd764a";

    public Test_BasketRepo(BasketWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _basketRepository = _applicationFactory.Services.GetRequiredService<IBasketRepository>();
    }

    [Fact]
    public async Task Test_UpdateDatabase_ShouldAddNewProductAngGetTheProduct()
    {
        var products = await _basketRepository.UpdateBasket(CreatedNewCustomerBasket());

        Assert.NotNull(products);
        Assert.Single(products.Items);
        Assert.Equal(userId, products.BuyerId);
    }

    private CustomerBasket CreatedNewCustomerBasket()
       => new CustomerBasket
       {
           BuyerId = userId,
           Items = new List<BasketItem>
           {
                new BasketItem
                {
                    Id = 3,
                    ProductId = 3,
                    ProductName = "Test 3",
                    PictureUrl = "test image url 3",
                    Quantity = 1,
                    UnitPrice = 12.55m,
                    OldUnitPrice = 11,
                }
           }
       };
}
