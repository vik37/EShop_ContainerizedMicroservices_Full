namespace EShop.Web.MVC.Services.Interfaces;

public interface IBasketService
{
    Task<Basket> GetBasket();
    Task<Basket> UpdateBasket(Basket basket);
    Task RemoveAllItems(string userId);

    Task<Order> GetOrderDraft(string basketId);
}
