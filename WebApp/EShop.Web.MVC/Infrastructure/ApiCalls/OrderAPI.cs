namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class OrderAPI
{
    public static string Base => "order";
    public static string GetOrderById(int id) => $"{Base}/{id}";
    public static string GetOrdersByUserId(string userId) => $"{Base}/{userId}";
    public static string GetCardTypes => $"{Base}/cardtypes";
    public static string CancelOrder => $"{Base}/cancel";
    public static string ShipOrder => $"{Base}/ship";
    public static string OrderDraft => $"{Base}/draft";
}