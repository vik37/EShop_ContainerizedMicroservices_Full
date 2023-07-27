namespace EShop.Orders.API.Application.IntegrationEvents.EventHangling;

public class GracePeriodConfirmedIntegrationEventHandler : IIntegrationEventHandler<GracedPeriodConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> _logger;

    public GracePeriodConfirmedIntegrationEventHandler(IMediator mediator, 
        ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(GracedPeriodConfirmedIntegrationEvent @event)
    {
        using(_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling Integration Event: {IntefrationEventId} - ({IntegrationEvent})",@event.Id,@event);

            var command = new SetAwaitingValidationOrderStatusCommand(@event.orderId);

            _logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({#Command)}",
                command.GetType().Name, nameof(command.OrderNumber), command.OrderNumber,command);

            await _mediator.Send(command);
        }
    }
}
