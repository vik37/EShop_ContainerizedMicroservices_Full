namespace Web.BFF.ShoppingAggregator.Models;

public class BasketData
{
    public string BuyerId { get; set; } = string.Empty;

    public List<BasketDataItems> Items { get; set; } = new();

    public BasketData() { }

    public BasketData(string buyerId)
    {
        BuyerId = buyerId;
    }
}