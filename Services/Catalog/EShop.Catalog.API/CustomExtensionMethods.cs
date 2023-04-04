using EShop.Catalog.API.Infrastructure;

namespace EShop.Catalog.API;

public static class CustomExtensionMethods
{
    public static WebApplication MigrateDbContext(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<CatalogDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogDbContext>>();
            if (logger is not null)
            {
                if (context is not null)
                    new CatalogContextSeed().SeedAsync(context, logger);
                else
                    logger.LogError("{ContextType} Migration Failed", nameof(CatalogDbContext));
            }

        }
        return app;
    }
}
