namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class PurchaseAPI
{
    public static string GetOrderDraft(string basketId) => $"order/draft/{basketId}";
}