
namespace EShop.Basket.API.Repository;

public interface IBasketRepository
{
    Task<CustomerBasket> GetProductFromBasketByUserId(string customerId);
    Task<CustomerBasket> UpdateBasket(CustomerBasket customerBasket);
    Task<bool> DeleteBasket(string id);
}
