using EventBus.Events;

namespace EShop.Basket.API.IntegrationEvents;

public record ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice) 
    : IntegrationEvent;
