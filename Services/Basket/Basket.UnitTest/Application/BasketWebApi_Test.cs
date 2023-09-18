namespace Basket.UnitTest.Application;

public class BasketWebApi_Test
{
    private readonly IBasketRepository _basketRepositorySub;
    private readonly ILogger<BasketController> _loggerSub;

    private readonly BasketController _basketController;
    public BasketWebApi_Test()
    {
        _basketRepositorySub = Substitute.For<IBasketRepository>();
        _loggerSub = Substitute.For<ILogger<BasketController>>();

        _basketController = new BasketController(_basketRepositorySub, _loggerSub);
    }

    [Fact]
    public async Task Test_GetBasketByCustomerId_ShouldBeSuccess()
    {
        // arrange
        string fakeBuyerId = "1";
        var fakeCustomerBasket = GetCustomerBasketFake(fakeBuyerId);

        _basketRepositorySub.GetProductFromBasketByUserId(Arg.Any<string>()).Returns(fakeCustomerBasket);

        // action
        var actionResult = await _basketController.GetProductFromBasketByUserId(fakeBuyerId);

        // assertion
        actionResult.Result.As<ObjectResult>().StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        actionResult.Result.As<ObjectResult>().Value.As<CustomerBasket>().BuyerId.Should().Be(fakeBuyerId);
    }

    [Theory]
    [InlineData("1",3)]
    [InlineData("2", 1)]
    public async Task Test_GetBasketByCustomerId_ShouldReturnBasketItemsByCustomerId(string buyerId, int itemCount)

    {
        // arrange
        var fakeBasketWithCustomerId1 = FakeBasketData.GetFakeBasketItems().SingleOrDefault(x => x!.BuyerId == buyerId);
        _basketRepositorySub.GetProductFromBasketByUserId(Arg.Any<string>()).Returns(fakeBasketWithCustomerId1);

        // action
        var actionResult = await _basketController.GetProductFromBasketByUserId(buyerId);

        // assertion
        actionResult.Result.As<ObjectResult>().StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        actionResult.Result.As<ObjectResult>().Value.As<CustomerBasket>().BuyerId.Should().Be(buyerId);
        actionResult.Result.As<ObjectResult>().Value.As<CustomerBasket>().Items.Should().HaveCount(itemCount,"Incorect count of items");
    }

    [Fact]
    public async Task Test_AddToBasketByCustomerId_ShouldBeSuccess()
    {
        // arrange
        string fakeBuyerId = "2";
        var fakeCustomerBasket = GetCustomerBasketFake(fakeBuyerId);
        _basketRepositorySub.UpdateBasket(Arg.Any<CustomerBasket>()).Returns(fakeCustomerBasket);

        // action
        var actionResult = await _basketController.AddNewProductToBasket(fakeCustomerBasket);

        // assertion
        actionResult.Result.As<ObjectResult>().StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        actionResult.Result.As<ObjectResult>().Value.As<CustomerBasket>().BuyerId.Should().Be(fakeBuyerId);
    }

    private static CustomerBasket GetCustomerBasketFake(string buyerId)
        => new ()
        {
            BuyerId = buyerId,
            Items = new List<BasketItem>()
        };
}

