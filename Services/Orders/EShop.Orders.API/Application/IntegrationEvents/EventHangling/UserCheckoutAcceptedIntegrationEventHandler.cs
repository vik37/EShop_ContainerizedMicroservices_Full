namespace EShop.Orders.API.Application.IntegrationEvents.EventHangling;

public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;
    private readonly IOrderIntegrationEventService _eventService;
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

    public UserCheckoutAcceptedIntegrationEventHandler(IOrderRepository orderRepository, IMediator mediator, 
        IOrderIntegrationEventService eventService, ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    ///     Integration Event Handler which starts the create order proccess
    /// </summary>
    /// <param name="event">
    ///     Integration event message which is sent by the
    ///     basket.api once it has successfully process the 
    ///     order items.
    /// </param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling Integration Event: {IntegrationEventId} - {(@IntegrationEvent)}", @event.Id, @event);

           bool result = false;

            if(@event.requestId != Guid.Empty)
            {
                using(_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommand", @event.requestId) }))
                {
                    var createdOrderCommand = new CreateOrderCommand(@event.customerBasket.Items,@event.userId,@event.userName,
                                                                    @event.city,@event.state,@event.street, @event.country,@event.zipCode,@event.cardNumber,
                                                                    @event.cardHolderName,@event.cardExpiration,@event.cardSecurityNumber,
                                                                    @event.cardTypeId);

                    var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createdOrderCommand, @event.requestId);

                    _logger.LogInformation("Sending Command: {CommandName} - {IdProperty}: {CommandId} {(@Command)}",
                                            requestCreateOrder.GetType().Name, nameof(requestCreateOrder.Id),requestCreateOrder.Id,requestCreateOrder);

                    result = await _mediator.Send(requestCreateOrder);

                    if (result)
                        _logger.LogInformation("CreateOrderCommand Succeeded - RequestId {RecuestId}", @event.requestId);
                    else
                        _logger.LogWarning("CreateOrderCommand Failed - RequestId {RequestId}", @event.requestId);
                }
            }
            else
            {
                _logger.LogWarning("Invalid Integration Event - RequestId is missing - {IntegrationEvent}", @event);
            }
        }
    }
}
