namespace Web.BFF.ShoppingAggregator.Services;

public interface IOrderService
{
    Task<OrderData?> GetOrderDraftAsync(BasketData basketData);
}