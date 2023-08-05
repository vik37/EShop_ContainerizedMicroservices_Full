namespace EShop.Basket.API.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
    private readonly IBasketRepository _basketRepository;

    public OrderStartedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger,
                                            IBasketRepository basketRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        using(LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{BasketApplication.GetApplication().ApplicationName}"))
        {
            _logger.LogInformation("------------ Handling integration event: {IntegrationEventId} - at {AppName} - {@IntegrationEvent}", @event.Id, BasketApplication.GetApplication().ApplicationName, @event);

            await _basketRepository.DeleteBasket(@event.userId);
        }
    }
}
