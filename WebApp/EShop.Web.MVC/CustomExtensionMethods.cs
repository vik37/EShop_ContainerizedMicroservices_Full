namespace EShop.Web.MVC;

public static class CustomExtensionMethods
{
    private static TimeSpan TimeSpan => TimeSpan.FromMinutes(4);

    public static IServiceCollection HttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<HttpCLientRequestIdDelegationHandler>();

        var httpUrlsOptionSettings = configuration.Get<APIUrlsOptionSettings>();

        services.AddHttpClient<ICatalogService, CatalogService>(httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(httpUrlsOptionSettings.GatewayAPI);
        })
          .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan });

        services.AddHttpClient<IBasketService, BasketService>(httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(httpUrlsOptionSettings.GatewayAPI);
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json");
        })
         .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan });

        services.AddHttpClient<IOrderService, OrderService>(httpConfig =>
        {
            httpConfig.BaseAddress = new Uri(httpUrlsOptionSettings.GatewayAPI);
            httpConfig.DefaultRequestHeaders.Add("Accept", "application/json");
        })
         .AddHttpMessageHandler<HttpCLientRequestIdDelegationHandler>()
         .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan });

        return services;
    }
}