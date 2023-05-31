namespace Basket.IntegrationTest;

public static class BasketURIPath
{
    public static string GetProductFromBasketByUserIdURIPath(string userId) => $"api/basket/{userId}";

    public static string AddNewProductToBasket => "api/basket";
}
