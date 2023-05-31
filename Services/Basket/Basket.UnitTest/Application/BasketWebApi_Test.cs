namespace Basket.UnitTest.Application;

public class BasketWebApi_Test
{
    private readonly Mock<IBasketRepository> _mockBasketRepo;
    private readonly Mock<ILogger<BasketController>> _loggerMock;

    public BasketWebApi_Test()
    {
        _mockBasketRepo = new Mock<IBasketRepository>();
        _loggerMock = new Mock<ILogger<BasketController>>();
    }

    [Fact]
    public async Task Test_GetBasketByCustomerId_ShouldBeSuccess()
    {
        // arrange
        string fakeBuyerId = "1";
        var fakeCustomerBasket = GetCustomerBasketFake(fakeBuyerId);

        _mockBasketRepo.Setup(x => x.GetProductFromBasketByUserId(It.IsAny<string>()))
            .ReturnsAsync(fakeCustomerBasket);

        // action
        var basketController = new BasketController(_mockBasketRepo.Object,_loggerMock.Object);
        var actionResult = await basketController.GetProductFromBasketByUserId(fakeBuyerId);

        // assertion

            //Assert.Equal((actionResult.Result as ObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            //Assert.Equal(((actionResult.Result as ObjectResult).Value as CustomerBasket).BuyerId, fakeBuyerId);

        // --- FLUENT ASSERTION -- \\
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
        
        _mockBasketRepo.Setup(x => x.GetProductFromBasketByUserId(It.IsAny<string>()))
            .ReturnsAsync(fakeBasketWithCustomerId1);

        // action
        
        var basketController = new BasketController(_mockBasketRepo.Object, _loggerMock.Object);
        var actionResult = await basketController.GetProductFromBasketByUserId(buyerId);

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

        _mockBasketRepo.Setup(x => x.UpdateBasket(It.IsAny<CustomerBasket>()))
            .ReturnsAsync(fakeCustomerBasket);

        // action
        var basketController = new BasketController(_mockBasketRepo.Object, _loggerMock.Object);
        var actionResult = await basketController.AddNewProductToBasket(fakeCustomerBasket);

        // assertion
        actionResult.Result.As<ObjectResult>().StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        actionResult.Result.As<ObjectResult>().Value.As<CustomerBasket>().BuyerId.Should().Be(fakeBuyerId);
    }

    private CustomerBasket GetCustomerBasketFake(string buyerId)
        => new CustomerBasket
        {
            BuyerId = buyerId,
            Items = new List<BasketItem>()
        };
}

