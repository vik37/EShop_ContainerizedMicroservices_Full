namespace Orders.UnitTest.Application;

public class Test_AdminOrdersWebApi
{
    private readonly IAdminOrderQuery _adminOrderQuerySub;
    private readonly IMediator _mediatorSub;
    private readonly ILogger<AdminOrdersController> _loggerSub;
    private readonly AdminOrdersController _adminOrdersController;

    public Test_AdminOrdersWebApi()
    {
        _adminOrderQuerySub = Substitute.For<IAdminOrderQuery>();
        _mediatorSub = Substitute.For<IMediator>();
        _loggerSub = Substitute.For<ILogger<AdminOrdersController>>();

        _adminOrdersController = new AdminOrdersController(_adminOrderQuerySub,_loggerSub,_mediatorSub);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {
        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var actionResult = await _adminOrdersController.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task CancelOrderWithRequestId_Success_StatusCodeShouldBeOk()
    {

        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var actionResult = await _adminOrdersController.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ShipOrderWithRequestId_Failed_StatusCodeShouldBeBadRequest()
    {
        // Arrange
        _mediatorSub.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var actionResult = await _adminOrdersController.ShipOrderAsync(new ShipOrderCommand(1), string.Empty) as BadRequestResult;

        // Assert
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }  
}