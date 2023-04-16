namespace EShop.Catalog.API;

public static class CustomExtensionMethods
{
    //************** Application Configurations  ******************\\

    public static IServiceCollection SwaggerConfigurations(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShopOnContainers - Catalog HTTP API",
                Version = "v1",
                Description = "The Catalog Microservice HTTP API. This is a DataDriven/CRUD microservice sample"
            });
        });

    public static IServiceCollection DatabaseConfiguration(this IServiceCollection services, IConfiguration config)=>
        services.AddDbContext<CatalogDbContext>(opt =>
        {
            if (config != null)
            {
                opt.UseSqlServer(Application.GetApplication().DockerMSQLConnectionString(config),
                sqlServerOptionsAction: sqlOption =>
                {
                    sqlOption.MigrationsAssembly(
                            Assembly.GetExecutingAssembly().GetName().Name
                        );
                    sqlOption.EnableRetryOnFailure(maxRetryCount: 5,
                                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                                    errorNumbersToAdd: null);
                });
            }
        });

    public static IServiceCollection CorsConfiguration(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("Catalog Cors", options =>
            {
                options.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
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
        
       
    //************** WEB APPLICATIONS  ******************\\

    public static WebApplication MigrateDbContext(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<CatalogDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogDbContext>>();
            if (logger is not null)
            {
                if (context is not null)
                    new CatalogContextSeed().Seed(context, logger);
                else
                    logger.LogError("{ContextType} Migration Failed", nameof(CatalogDbContext));
            }

        }
        return app;
    }
}
