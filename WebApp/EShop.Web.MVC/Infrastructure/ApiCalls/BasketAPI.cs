namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class BasketAPI
{
    public static string BasketBaseURIPath => "basket";
    /// <summary>
    /// URI path helper.
    /// REMEMBER: The default "userid" value is temporary, until an user security server will be created.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>URI path</returns>
    public static string GetBasketByUserIdURIPath(string userId = "9899b909-e395-47a5-914e-676d9602942a") => $"{BasketBaseURIPath}/{userId}";

    public static string RemoveBasketByUserId(string userId) => $"{BasketBaseURIPath}/{userId}";
}
