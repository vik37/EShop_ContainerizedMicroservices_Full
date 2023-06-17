namespace EShop.Web.MVC;

public static class CustomExtensionMethods
{
    private static TimeSpan ts => TimeSpan.FromMinutes(4);

    public static IServiceCollection HttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddHttpClient<ICatalogService, CatalogService>("CatalogAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(Application.GetApplication().GatewayAPI(configuration));
            httpConfig.Timeout = ts;
        });

        services.AddHttpClient<IBasketService, BasketService>("BasketAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(Application.GetApplication().GatewayAPI(configuration));
            httpConfig.Timeout = ts;
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}