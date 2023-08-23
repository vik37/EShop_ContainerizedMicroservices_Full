namespace EShop.Web.MVC;

public static class CustomExtensionMethods
{
    private static TimeSpan TimeSpan => TimeSpan.FromMinutes(4);

    public static IServiceCollection HttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddHttpClient<ICatalogService, CatalogService>("CatalogAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(Application.GetApplication().GatewayAPI(configuration));
            httpConfig.Timeout = TimeSpan;
        });

        services.AddHttpClient<IBasketService, BasketService>("BasketAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(Application.GetApplication().GatewayAPI(configuration));
            httpConfig.Timeout = TimeSpan;
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}