namespace EShop.Orders.API.Application.Models;

public class CustomBasket
{
    public string BuyerId { get; set; } = string.Empty;
    public List<BasketItem> Items { get; set; }

    public CustomBasket(string buyerId, List<BasketItem> basketItems)
    {
        BuyerId = buyerId;
        Items = basketItems;
    }
}
