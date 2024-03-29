﻿namespace EShop.Orders.API.Application.IntegrationEvents.EventHangling;

public class OrderStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStockConfirmedIntegrationEventHandler> _logger;

    public OrderStockConfirmedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            var command = new SetStockConfirmedOrderStatusCommand(@event.orderId);

            _logger.LogInformation("Sending command: {CommandName} - {IdProperty}: {CommandId} ({#Command)}",
                command.GetType().Name, nameof(command.OrderNumber), command.OrderNumber, command);

            await _mediator.Send(command);
        }
    }
}
