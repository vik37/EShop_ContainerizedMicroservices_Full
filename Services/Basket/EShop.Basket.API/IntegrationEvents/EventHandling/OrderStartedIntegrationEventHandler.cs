using EShop.Basket.API.IntegrationEvents.Events;
using EventBus.Abstractions;
using Serilog.Context;

namespace EShop.Basket.API.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly ILogger<ProductPriceChangedIntegrationEventHandling> _logger;
    private readonly BasketRepository _basketRepository;

    public OrderStartedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandling> logger,
                                            BasketRepository basketRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        using(LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Application.GetApplication().ApplicationName}"))
        {
            _logger.LogInformation("------------ Handling integration event: {IntegrationEventId} - at {AppName} - {@IntegrationEvent}", @event.Id, Application.GetApplication().ApplicationName, @event);

            await _basketRepository.DeleteBasket(@event.userId);
        }
    }
}
