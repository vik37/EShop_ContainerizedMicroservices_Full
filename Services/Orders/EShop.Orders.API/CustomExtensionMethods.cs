namespace EShop.Orders.API;

public static class CustomExtensionMethods
{
    //************** Application Configurations  ******************\\

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

    public static IServiceCollection DatabaseConfiguration(this IServiceCollection services, string connectionString)
    {

        static void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions)
        {
            sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);

            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15,maxRetryDelay: TimeSpan.FromSeconds(30),errorNumbersToAdd:null);
        }
        
        if(!string.IsNullOrEmpty(connectionString)) 
        {
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(connectionString, ConfigureSqlOptions);
            });

            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseSqlServer(connectionString, ConfigureSqlOptions);
            });
        }

        return services;
    }

    public static IServiceCollection ConfigurationEventBus(this IServiceCollection services, EventBusSettings eventBusSettings, int port = 0)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp =>
             (DbConnection dc) => new IntegrationEventLogService(dc));
        services.AddTransient<IOrderIntegrationEventService, OrderIntegrationEventService>();

        services.AddSingleton<IRabbitMQPersistentConnection>(rpc =>
        {
            var logger = rpc.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

            ConnectionFactory factory = new()
            {
                HostName = eventBusSettings.rabbitMQConnection,
                DispatchConsumersAsync = true,
                UserName = eventBusSettings.eventBusRabbitMQUsername,
                Password = eventBusSettings.eventBusRabbitMQPassword,
            };

            if(port > 0)
                factory.Port = port;

            return new RabbitMQPersistentConnection(connectionFactory: factory, logger: logger, retryCount: eventBusSettings.eventBusRetry);
        });
        
        return services;
    }

    public static IServiceCollection RegisterEventBusRabbitMQ(this IServiceCollection services, EventBusSettings eventBusSettings)
    {
        services.AddSingleton<IEventBus, EventBusRabbitMQ>(reb =>
        {

            var rabbitMQPersistentConnetion = reb.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = reb.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var rabbitMQEventBusSubscriptionManager = reb.GetRequiredService<IEventBusSubscriptionManager>();

            return new EventBusRabbitMQ(persistentConnection: rabbitMQPersistentConnetion, logger: logger, queueName: eventBusSettings.subscriptionClientName,
                    eventBusSubscriptionManager: rabbitMQEventBusSubscriptionManager, serviceProvider: reb, retryCount: eventBusSettings.eventBusRetry);
        });
        services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();

        return services;
    }

    //************** APPLICATION BUILDER ******************\\

    public static WebApplication MigrateDbContext(this WebApplication app)
    {
        using(var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<OrderContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContext>>();

            if(logger is not null)
            {
                if (context is not null)
                    new OrderContextSeed().Seed(context,logger);
                else
                    logger.LogError("{ContextType} Migration Failed", nameof(OrderContext));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<IntegrationEventLogDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IntegrationEventLogDbContext>>();
            if (logger is not null)
            {
                if (context is not null)
                {
                    logger.LogInformation("{ContextType} Start with Migration", nameof(IntegrationEventLogDbContext));
                    context.Database.Migrate();
                    logger.LogInformation("{ContextType} End with Migration", nameof(IntegrationEventLogDbContext));
                }
                else
                    logger.LogError("{ContextType} Migration Failed", nameof(IntegrationEventLogDbContext));
            }

        }

        return app;
    }
}
