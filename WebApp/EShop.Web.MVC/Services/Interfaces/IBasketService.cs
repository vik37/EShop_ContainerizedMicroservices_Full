namespace EShop.Web.MVC.Services.Interfaces;

public interface IBasketService
{
    Task<Basket> GetBasket();
    Task<Basket> UpdateBasket(Basket basket);
}
