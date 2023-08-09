namespace Orders.UnitTest.Application;

public class Test_NewOrderRequestHandlerTest : FakeOrderRequestWithBuyer
{
    private readonly IOrderRepository _orderRepositorySub;
    private readonly IMediator _mediatorSub;
    private readonly IOrderIntegrationEventService _orderIntegrationEventServiceSub;

    public Test_NewOrderRequestHandlerTest()
    {
        _orderRepositorySub = Substitute.For<IOrderRepository>();
        _mediatorSub = Substitute.For<IMediator>();
        _orderIntegrationEventServiceSub = Substitute.For<IOrderIntegrationEventService>();
    }

    [Fact]
    public async Task HandleReturnFalse_IfOrderIsNotPersistet()
    {
        // Arrange
        var fakeOrderCommand = FakeOrderRequest(new Dictionary<string, object>
        {
            ["cardExpirationDate"] = DateTime.Now.AddYears(1),
        });

        _orderRepositorySub.GetAsync(Arg.Any<int>()).Returns(Task.FromResult(FakeOrder()));
        _orderRepositorySub.UnitOfWork.SaveChangesAsync(default).Returns(Task.FromResult(1));

        var loggerSub = Substitute.For<ILogger<CreateOrderCommandHandler>>();
     
        // Action
        var handler = new CreateOrderCommandHandler(_orderRepositorySub, _mediatorSub, _orderIntegrationEventServiceSub, loggerSub);
        var cancellationToken = new CancellationToken();
        var result = await handler.Handle(fakeOrderCommand, cancellationToken);

        // Assertion
        result.Should().BeFalse();
    }

    private Order FakeOrder()
        => new Order("1", "fake name", new Address(
                                        "street", "city", "state", "country", "zipCode"),
                    1, "123456", "1890", "123", DateTime.Now.AddYears(1));
}