namespace EShop.Web.MVC;

public static class CustomExtensionMethods
{
    private static TimeSpan ts => TimeSpan.FromMinutes(4);

    public static IServiceCollection HttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddHttpClient<ICatalogService, CatalogService>("CatalogAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(configuration["CatalogAPIDocker"]);
            httpConfig.Timeout = ts;
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json; ver=1.0");
        });

        services.AddHttpClient<IBasketService, BasketService>("BasketAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(configuration["BasketAPIDocker"]);
            httpConfig.Timeout = ts;
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}