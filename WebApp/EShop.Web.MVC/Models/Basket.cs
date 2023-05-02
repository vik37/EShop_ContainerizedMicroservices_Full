namespace EShop.Web.MVC.Models;

public class Basket
{
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    public decimal Total()
    {
        return Math.Round(Items.Sum(i => i.UnitPrice*i.Quantity),2);
    }
}