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

    public static IServiceCollection ConfigurationEventBus(this IServiceCollection services, IConfiguration configuration = null,
                                        int retryConnection = 5, string connectionUri = null)
    {
        services.AddSingleton<IRabbitMQPersistentConnection>(rpc =>
        {
            var logger = rpc.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

            ConnectionFactory factory = null;
            if(configuration is not null)
            {
                factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusRabbitMQUsername"]))
                    factory.UserName = configuration["EventBusRabbitMQUsername"];

                if (!string.IsNullOrEmpty(configuration["EventBusRabbitMQPassword"]))
                    factory.Password = configuration["EventBusRabbitMQPassword"];
            }
            else
            {
                factory = new()
                {
                    Uri = new Uri(connectionUri)
                };
            }
            return new RabbitMQPersistentConnection(connectionFactory: factory,logger: logger,retryCount: retryConnection);
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
