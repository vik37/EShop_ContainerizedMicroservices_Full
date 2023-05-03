namespace EShop.Web.MVC.Infrastructure;

public class Retry
{
    public AsyncRetryPolicy<HttpResponseMessage> CreatePolicy(int count = 3) =>
        Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(count, retryAttempt => TimeSpan.FromSeconds(3));
}
