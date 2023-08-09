namespace Orders.UnitTest.Application;

public class Test_IdentifiedCommandHandler : FakeOrderRequestWithBuyer
{
    private readonly IMediator _mediatorSub;
    private readonly IRequestManager _requestManagerSub;
    private readonly ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> _loggerSub;

    public Test_IdentifiedCommandHandler()
    {
        _mediatorSub = Substitute.For<IMediator>();
        _requestManagerSub = Substitute.For<IRequestManager>();
        _loggerSub = Substitute.For<ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>>>();
    }

    [Fact]
    public async Task HandlerSendsCommandWhenOrderNoExist()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCommand = new IdentifiedCommand<CreateOrderCommand,bool>(FakeOrderRequest(),fakeGuid);

        _requestManagerSub.ExistAsync(Arg.Any<Guid>()).Returns(Task.FromResult(false));
        _mediatorSub.Send(Arg.Any<IRequest<bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var handler = new CreateOrderIdentifiedCommandHandler(_mediatorSub,_requestManagerSub,_loggerSub);
        var result = await handler.Handle(fakeOrderCommand,CancellationToken.None);

        // Assert
        Assert.True(result);
        await _mediatorSub.Received(1).Send(Arg.Any<IRequest<bool>>(),default);
    }

    [Fact]
    public async void HandlerSendsNoCommandWhenOrderAlreadyExist()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCommand = new IdentifiedCommand<CreateOrderCommand,bool>(FakeOrderRequest(), fakeGuid);

        _requestManagerSub.ExistAsync(Arg.Any<Guid>()).Returns(Task.FromResult(true));
        _mediatorSub.Send(Arg.Any<IRequest<bool>>(), default).Returns(Task.FromResult(true));

        // Action
        var handler = new CreateOrderIdentifiedCommandHandler(_mediatorSub,_requestManagerSub, _loggerSub);
        await handler.Handle(fakeOrderCommand, CancellationToken.None);

        // Assert
        _mediatorSub.DidNotReceive().Send(Arg.Any<IRequest<bool>>(),default).Wait();
    }
}