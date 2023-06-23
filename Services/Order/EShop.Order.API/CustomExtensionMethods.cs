namespace EShop.Order.API;

public static class CustomExtensionMethods
{
    public static IServiceCollection SwaggerConfigurations(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShopOnContainers - Ordering HTTP API",
                Version = "v1",
                Description = "The Ordering Microservice HTTP API"
            });
        });
}
