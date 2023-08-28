namespace EShop.Web.MVC.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;
    protected readonly AsyncRetryPolicy<HttpResponseMessage> _policy;

    public BaseService(HttpClient httpClient, Retry retry)
    {
        _httpClient = httpClient;
        _policy = retry.CreatePolicy(5);
    }
}
