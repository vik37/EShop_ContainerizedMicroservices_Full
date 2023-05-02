using StackExchange.Redis;

namespace EShop.Basket.API;

public static class CustomApplicationExtensionMethods
{
    public static IServiceCollection SwaggerConfigurations(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShopOnContainers - Catalog HTTP API",
                Version = "v1",
                Description = "The Basket Microservice HTTP API"
            });
        });

    public static IServiceCollection CorsConfiguration(this IServiceCollection services) =>
       services.AddCors(options =>
       {
           options.AddPolicy("Basket Cors", options =>
           {
               options.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
           });
       });

    public static IServiceCollection RedisConnectionMultyplexer(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton<IConnectionMultiplexer>(OptionsBuilderConfigurationExtensions =>
            ConnectionMultiplexer.Connect(configuration: configuration["RedisConnectionString"])
        );

}
