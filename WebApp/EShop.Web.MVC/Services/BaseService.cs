namespace EShop.Web.MVC.Services;

public class BaseService
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly AsyncRetryPolicy<HttpResponseMessage> _policy;


    public BaseService(IHttpClientFactory httpClientFactory, Retry retry)
    {
        _httpClientFactory = httpClientFactory;
        _policy = retry.CreatePolicy(5);
    }
}
