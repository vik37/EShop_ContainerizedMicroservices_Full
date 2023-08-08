namespace Orders.UnitTest.Application;

public class Test_NewOrderRequestHandlerTest : FakeOrderRequestWithBuyer
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IOrderIntegrationEventService> _orderIntegrationEventService;

    public Test_NewOrderRequestHandlerTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _mediator = new Mock<IMediator>();
        _orderIntegrationEventService = new Mock<IOrderIntegrationEventService>();
    }

    [Fact]
    public async Task HandleReturnFalse_IfOrderIsNotPersistet()
    {
        // Arrange
        var fakeOrderCommand = FakeOrderRequest(new Dictionary<string, object>
        {
            ["cardExpirationDate"] = DateTime.Now.AddYears(1),
        });

        _orderRepository.Setup(orderRepo => orderRepo.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(FakeOrder()));
        _orderRepository.Setup(buyerRepo => buyerRepo.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

        var logger = new Mock<ILogger<CreateOrderCommandHandler>>();

        // Action
        var handler = new CreateOrderCommandHandler(_orderRepository.Object, _mediator.Object, _orderIntegrationEventService.Object, logger.Object);
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