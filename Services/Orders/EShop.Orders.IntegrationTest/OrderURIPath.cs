namespace EShop.Orders.IntegrationTest;

public static class OrderURIPath
{
    public const string DefaultOrderURIPath = "/api/v1/order";

    public const string DefaultAdminOrderURIPath = "/api/v1/adminorder";

    public static string GetAllOrdersForAdministrator() => $"{DefaultOrderURIPath}";

    public static string GetOrderById(int id) => $"/{Guid.NewGuid().ToString()}/user/{id}";

    public static string GetAllCardTypes => string.Concat(DefaultOrderURIPath, "/cardtypes");

    public static string CancelOrder => string.Concat(DefaultOrderURIPath, "/cancel");

    public static string ShipOrder => string.Concat(DefaultAdminOrderURIPath, "/ship");

}