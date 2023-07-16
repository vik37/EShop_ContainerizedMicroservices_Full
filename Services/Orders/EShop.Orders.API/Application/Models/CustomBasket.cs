namespace EShop.Orders.API.Application.Models;

public class CustomBasket
{
    public string BuyerId { get; set; } = string.Empty;
    public List<BasketItems> Items { get; set; }

    public CustomBasket(string buyerId, List<BasketItems> basketItems)
    {
        BuyerId = buyerId;
        Items = basketItems;
    }
}
