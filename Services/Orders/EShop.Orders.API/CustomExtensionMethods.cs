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

    public static IServiceCollection ConfigureIntegrationeventServices(this IServiceCollection services)
    {

        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp =>
             (DbConnection dc) => new IntegrationEventLogService(dc));

        services.AddTransient<IOrderIntegrationEventService, OrderIntegrationEventService>();


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

        return app;
    }
}
