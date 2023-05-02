namespace EShop.Web.MVC.Infrastructure.ApiCalls;

public static class BasketAPI
{
    /// <summary>
    /// URI path helper.
    /// REMEMBER: The default "userid" value is temporary, until an user security server will be created.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>URI path</returns>
    public static string GetBasketByUserIdURIPath(string userId = "9899b909-e395-47a5-914e-676d9602942a") => $"{userId}"; 
}
