namespace Web.BFF.ShoppingAggregator.Services;

public interface IBasketService
{
    Task<BasketData?> GetBasketByIdAsync(string id);
}