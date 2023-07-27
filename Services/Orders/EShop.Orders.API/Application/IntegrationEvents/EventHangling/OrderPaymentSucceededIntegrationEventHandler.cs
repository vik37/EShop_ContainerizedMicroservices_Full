namespace EShop.Orders.API.Application.IntegrationEvents.EventHangling;

public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderPaymentSucceededIntegrationEventHandler> _logger;

    public OrderPaymentSucceededIntegrationEventHandler(IMediator mediator, ILogger<OrderPaymentSucceededIntegrationEventHandler> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            var command = new SetPaidOrderStatusCommand(@event.orderId);

            _logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({#Command)}",
                command.GetType().Name, nameof(command.OrderNumber), command.OrderNumber, command);

            await _mediator.Send(command);
        }
    }
}