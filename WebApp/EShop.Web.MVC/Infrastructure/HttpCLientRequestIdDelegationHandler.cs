namespace EShop.Web.MVC.Infrastructure;

public class HttpCLientRequestIdDelegationHandler : DelegatingHandler
{
    public HttpCLientRequestIdDelegationHandler()
    { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(request.Method ==  HttpMethod.Post || request.Method == HttpMethod.Post)
        {
            if (!request.Headers.Contains("x-requestid"))
            {
                request.Headers.Add("x-requestid", Guid.NewGuid().ToString());
            }
        }
        return await base.SendAsync(request, cancellationToken);
    }
}