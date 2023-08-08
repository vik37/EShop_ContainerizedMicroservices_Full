namespace Orders.UnitTest.Application;

public class Test_IdentifiedCommandHandler : FakeOrderRequestWithBuyer
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IRequestManager> _requestManager;
    private readonly Mock<ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>>> _logger;

    public Test_IdentifiedCommandHandler()
    {
        _mediator = new Mock<IMediator>();
        _requestManager = new Mock<IRequestManager>();
        _logger = new Mock<ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>>>();
    }

    [Fact]
    public async Task HandlerSendsCommandWhenOrderNoExist()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCommand = new IdentifiedCommand<CreateOrderCommand,bool>(FakeOrderRequest(),fakeGuid);

        _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));
        _mediator.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default)).Returns(Task.FromResult(true));

        // Action
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator.Object,_requestManager.Object,_logger.Object);
        var result = await handler.Handle(fakeOrderCommand,CancellationToken.None);

        // Assert
        Assert.True(result);
        _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(),default), Times.Once);
    }

    [Fact]
    public async void HandlerSendsNoCommandWhenOrderAlreadyExist()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCommand = new IdentifiedCommand<CreateOrderCommand,bool>(FakeOrderRequest(), fakeGuid);

        _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
        _mediator.Setup(x => x.Send(It.IsAny<IRequest<bool>>(),default)).Returns(Task.FromResult(true));

        // Action
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator.Object,_requestManager.Object, _logger.Object);
        var result = await handler.Handle(fakeOrderCommand, CancellationToken.None);

        // Assert
        _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), default), Times.Never());
    }
}