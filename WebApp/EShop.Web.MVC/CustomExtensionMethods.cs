namespace EShop.Web.MVC;

public static class CustomExtensionMethods
{
    public static IServiceCollection HttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICatalogService, CatalogService>("CatalogAPI", httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(configuration["CatalogAPIDocker"]);
            httpConfig.Timeout = TimeSpan.FromMinutes(4);
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json; ver=1.0");
        });

        return services;
    }
}