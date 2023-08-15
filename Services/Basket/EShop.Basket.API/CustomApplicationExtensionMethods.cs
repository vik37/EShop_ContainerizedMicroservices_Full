using Microsoft.AspNetCore.Mvc.Versioning;

namespace EShop.Basket.API;

public static class CustomApplicationExtensionMethods
{
    //************** Application Configurations  ******************\\
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

    public static IServiceCollection ApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                );
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

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

    public static IServiceCollection RedisConnectionMultyplexer(this IServiceCollection services, string redisConnectionString) =>
        services.AddSingleton<IConnectionMultiplexer>(OptionsBuilderConfigurationExtensions =>
            ConnectionMultiplexer.Connect(configuration: redisConnectionString)
        );

    public static IServiceCollection ConfigurationEventBus(this IServiceCollection services, string rabbitConnection,
                                         string rabbitUsername, string rabbitPassword, string port = null, int retryConnection = 5)
    {
        services.AddSingleton<IRabbitMQPersistentConnection>(rpc =>
        {
            var logger = rpc.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

            ConnectionFactory factory = new()
            {
                HostName = rabbitConnection,
                DispatchConsumersAsync = true,
                UserName = rabbitUsername,
                Password = rabbitPassword,
            };

            if (!string.IsNullOrEmpty(port))
            {
                bool isPortNumber = int.TryParse(port, out int num);
                if (isPortNumber)
                    factory.Port = num;
                else
                    logger.LogError("Port must be of type number. Port {Port} - Type {PortType}", port, port.GetType().Name);
            }

            return new RabbitMQPersistentConnection(connectionFactory: factory, logger: logger, retryCount: retryConnection);
        });
        return services;
    }

    public static IServiceCollection RegisterEventBusRabbitMQ(this IServiceCollection services, 
                                            string subscriptionClientName, int retryConnection = 5)
    {
        services.AddSingleton<IEventBus, EventBusRabbitMQ>(reb =>
        {
            
            var rabbitMQPersistentConnetion = reb.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = reb.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var rabbitMQEventBusSubscriptionManager = reb.GetRequiredService<IEventBusSubscriptionManager>();

            return new EventBusRabbitMQ(persistentConnection: rabbitMQPersistentConnetion,logger: logger, queueName: subscriptionClientName,
                    eventBusSubscriptionManager: rabbitMQEventBusSubscriptionManager, serviceProvider: reb, retryCount: retryConnection);
        });
        services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();
        services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
        services.AddTransient<OrderStartedIntegrationEventHandler>();

        return services;
    }
}
