namespace Orders.UnitTest.Application;

public class Test_OrdersWebApi
{
    private readonly Mock<IOrderQuery> _orderQuery;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<OrderController>> _logger;

    public Test_OrdersWebApi()
    {
        _orderQuery = new Mock<IOrderQuery>();
        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<OrderController>>();
    }

    [Fact]
    public async Task CancelOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {
        // Arrange
        _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default))
                    .Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);

    }

    [Fact]
    public async Task CancelOrderWithRequestId_Failed_StatusCodeShouldBeBadRequest()
    {
        // Arrange
        _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default))
                    .Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.CancelOrderAsync(new CancelOrderCommand(1), string.Empty) as BadRequestResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {
        // Arrange
        _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default))
                            .Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Failed_StatusCodeShouldBeBadRequest()
    {
        // Arrange
        _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default))
                            .Returns(Task.FromResult(true));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.ShipOrderAsync(new ShipOrderCommand(1), string.Empty) as BadRequestResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task GetOrderFromUserId_Success_ShouldReturnStatusCodeOk()
    {
        // Arrange
        var fakeOrderSummary = Enumerable.Empty<OrderSummaryViewModel>();

        _orderQuery.Setup(x => x.GetOrdersFromUserAsync(Guid.NewGuid())).Returns(Task.FromResult(fakeOrderSummary));

        // Action
        var orderController = new OrderController(_orderQuery.Object,_mediator.Object, _logger.Object);
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
        
        _orderQuery.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(fakeOrder));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.GetOrderByIdAsync(fakeOrderId);

        var a = actionResult.Value;
        // Assert
        actionResult.Value.Should().BeOfType<OrderViewModel>().And.BeSameAs(fakeOrder).And.BeEquivalentTo(fakeOrder);
    }

    [Fact]
    public async Task GetOrderByOrderId_Failed_ShouldReturnStatusCodeNotFound()
    {
        // Arrange
        int fakeOrderId = 123;
        var fakeOrder = new OrderViewModel();

        _orderQuery.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).Throws(new KeyNotFoundException());

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
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

        _orderQuery.Setup(x => x.GetCardTypesAsync()).Returns(Task.FromResult(fakeCardType));

        // Action
        var orderController = new OrderController(_orderQuery.Object, _mediator.Object, _logger.Object);
        var actionResult = await orderController.GetAllCartTypes();

        // Assert
        actionResult.Result.As<OkObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
