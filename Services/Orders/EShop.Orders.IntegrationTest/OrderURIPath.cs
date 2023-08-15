namespace EShop.Orders.IntegrationTest;

public static class OrderURIPath
{
    public const string DefaultURIPath = "/api/v1/order";

    public static string GetOrderById(int id) => string.Concat(DefaultURIPath, "/",id.ToString());

    public static string GetAllCardTypes => string.Concat(DefaultURIPath, "/cardtypes");

    public static string CancelOrder => string.Concat(DefaultURIPath, "/cancel");

    public static string ShipOrder => string.Concat(DefaultURIPath, "/ship");

}