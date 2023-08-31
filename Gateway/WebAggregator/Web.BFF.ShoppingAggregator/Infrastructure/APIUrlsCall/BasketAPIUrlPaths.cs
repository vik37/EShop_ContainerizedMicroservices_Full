namespace Web.BFF.ShoppingAggregator.Infrastructure.APIUrlsCall;

public static class BasketAPIUrlPaths
{
    public static string GetBasketByUserIdAsync(string userId) => $"/basket/{userId}";
}