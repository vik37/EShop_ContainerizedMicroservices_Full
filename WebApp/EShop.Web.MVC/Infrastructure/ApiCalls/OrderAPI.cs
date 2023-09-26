namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class OrderAPI
{
    public static string Base => "order";
    public static string GetOrderById(string userId,int orderId) => $"{Base}/{userId}/user/{orderId}";
    public static string GetOrdersByUserId(string userId) => $"{Base}/user/{userId}";
    public static string GetCardTypes => $"{Base}/cardtypes";
    public static string Create => $"{Base}/create";
    public static string CancelOrder => $"{Base}/cancel";
    public static string ShipOrder => $"{Base}/ship";
}