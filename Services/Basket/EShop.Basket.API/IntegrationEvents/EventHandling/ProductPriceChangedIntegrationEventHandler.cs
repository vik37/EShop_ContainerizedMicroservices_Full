namespace EShop.Basket.API.IntegrationEvents.EventHandling;

public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
{
    private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
    private readonly IBasketRepository _basketRepository;

    public ProductPriceChangedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger,
                                                        IBasketRepository basketRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    public async Task Handle(ProductPriceChangedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("Integration Event Context", $"{@event.Id}-{BasketApplication.GetApplication().ApplicationName}"))
        {
            _logger.LogInformation("------------ Handling integration event: {IntegrationEventId} - at {AppName} - {@IntegrationEvent}", @event.Id, BasketApplication.GetApplication().ApplicationName, @event);

            // Temporarely until an user security server will be created.
            string userId = "9899b909-e395-47a5-914e-676d9602942a";

            var basket = await _basketRepository.GetProductFromBasketByUserId(userId);
            if(basket is not null)
            {
                await UpdatePriceInBasketItems(@event.productId, @event.newPrice, @event.oldPrice, basket);
            }
        }
    }

    private async Task UpdatePriceInBasketItems(int productId, decimal newPrice, decimal oldPrice, CustomerBasket basket)
    {
        var itemsToUpdate = basket.Items.Where(b => b.ProductId == productId).ToList();
        if(itemsToUpdate is not null)
        {
            _logger.LogInformation("------ ProductPriceChangedIntegrationEventHandling: Updateing items to Basket for user {buyerId}", basket.BuyerId);

            foreach(var item in itemsToUpdate)
            {
                if(item.UnitPrice != newPrice)
                {
                    item.Id = productId;
                    item.UnitPrice = newPrice;
                    item.OldUnitPrice = oldPrice;
                }
            }
            await _basketRepository.UpdateBasket(basket);
        }
    }
}
