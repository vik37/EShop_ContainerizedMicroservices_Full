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

    public static IServiceCollection RedisConnectionMultyplexer(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton<IConnectionMultiplexer>(OptionsBuilderConfigurationExtensions =>
            ConnectionMultiplexer.Connect(configuration: configuration["RedisConnectionString"])
        );

    public static IServiceCollection ConfigurationEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMQPersistentConnection>(rpc =>
        {
            var logger = rpc.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQConnection"],
                DispatchConsumersAsync = true
            };

            if (!string.IsNullOrEmpty(configuration["EventBusRabbitMQUsername"]))
                factory.UserName = configuration["EventBusRabbitMQUsername"];

            if (!string.IsNullOrEmpty(configuration["EventBusRabbitMQPassword"]))
                factory.Password = configuration["EventBusRabbitMQPassword"];

            int retryConnection = 5;
            if (!string.IsNullOrEmpty(configuration["EventBusRetry"]))
                retryConnection = int.Parse(configuration["EventBusRetry"]);

            return new RabbitMQPersistentConnection(connectionFactory: factory,logger: logger,retryCount: retryConnection);

        });
        return services;
    }

    public static IServiceCollection RegisterEventBusRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus, EventBusRabbitMQ>(reb =>
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];
            var rabbitMQPersistentConnetion = reb.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = reb.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var rabbitMQEventBusSubscriptionManager = reb.GetRequiredService<IEventBusSubscriptionManager>();

            int retryCount = 5;
            if (!string.IsNullOrEmpty(configuration["EventBusRetry"]))
                retryCount = int.Parse(configuration["EventBusRetry"]);

            return new EventBusRabbitMQ(persistentConnection: rabbitMQPersistentConnetion,logger: logger, queueName: subscriptionClientName,
                    eventBusSubscriptionManager: rabbitMQEventBusSubscriptionManager, serviceProvider: reb, retryCount: retryCount);
        });
        services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();
        services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
        services.AddTransient<OrderStartedIntegrationEventHandler>();

        return services;
    }
}
