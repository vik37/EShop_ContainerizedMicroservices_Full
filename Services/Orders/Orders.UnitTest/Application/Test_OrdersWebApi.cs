namespace Orders.UnitTest.Application;

public class Test_OrdersWebApi
{
    private readonly IOrderQuery _orderQuerySub;
    private readonly IMediator _mediatorSub;
    private readonly ILogger<OrderController> _loggerSub;

    public Test_OrdersWebApi()
    {
        _orderQuerySub = Substitute.For<IOrderQuery>();
        _mediatorSub = Substitute.For<IMediator>();
        _loggerSub = Substitute.For<ILogger<OrderController>>();
    }

    [Fact]
    public async Task CancelOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {

        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand,bool>>(),default).Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuerySub,_mediatorSub,_loggerSub);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task CancelOrderWithRequestId_Failed_StatusCodeShouldBeBadRequest()
    {
        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), string.Empty) as BadRequestResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {
        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Failed_StatusCodeShouldBeBadRequest()
    {
        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), string.Empty) as BadRequestResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task GetOrdersFromUserId_Success_ShouldReturnStatusCodeOk()
    {
        // Arrange
        var fakeOrderSummary = Enumerable.Empty<OrderSummaryViewModel>();
        _orderQuerySub.GetOrdersFromUserAsync(Guid.NewGuid()).Returns(Task.FromResult(fakeOrderSummary));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.GetAllOrdersByUser();

        // Assert
        actionResult.Result.As<OkObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);        
    }

    [Fact]
    public async Task GetOrderByOrderId_Success()
    {
        // Arrange
        int fakeOrderId = 123;
        var fakeOrder = new OrderViewModel();

        _orderQuerySub.GetOrderByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fakeOrder));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.GetOrderByIdAsync(fakeOrderId);

        // Assert
        actionResult.Value.Should().BeOfType<OrderViewModel>().And.BeSameAs(fakeOrder).And.BeEquivalentTo(fakeOrder);
    }

    [Fact]
    public async Task GetOrderByOrderId_Failed_ShouldReturnStatusCodeNotFound()
    {
        // Arrange
        int fakeOrderId = 123;

        _orderQuerySub.GetOrderByIdAsync(Arg.Any<int>()).Throws(new KeyNotFoundException());

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.GetOrderByIdAsync(fakeOrderId);

        var notFoundResult = actionResult.Result as NotFoundResult;
        // Assert
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetCardType_Success()
    {
        // Arrange
        var fakeCardType = Enumerable.Empty<CardTypeViewModel>();

        _orderQuerySub.GetCardTypesAsync().Returns(Task.FromResult(fakeCardType));

        // Action
        var orderController = new OrderController(_orderQuerySub, _mediatorSub, _loggerSub);
        var actionResult = await orderController.GetAllCartTypes();

        // Assert
        actionResult.Result.As<OkObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
