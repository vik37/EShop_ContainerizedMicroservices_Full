namespace Basket.IntegrationTest;

public static class BasketURIPath
{
    public static string GetProductFromBasketByUserIdURIPath(string userId) => $"api/v1/basket/{userId}";

    public static string AddNewProductToBasket => "api/v1/basket";
}
