using EShop.Catalog.API.Entities;
using Microsoft.Data.SqlClient;
using Polly.Retry;
using Polly;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.API.Infrastructure;

public class CatalogContextSeed
{
    public void SeedAsync(CatalogDbContext dbContext, ILogger<CatalogDbContext> logger)
    {
        logger.LogInformation("{ContextType} Migration Seed Start", nameof(CatalogDbContext));
        var policy = CreatePolicy(logger);
        policy.Execute(() =>
        {
            if (dbContext != null)
            {
                dbContext.Database.Migrate();
                if (!dbContext.CatalogBrands.Any())
                {
                    dbContext.CatalogBrands.AddRange(GetPreconfiguredCatalogBrands);
                    dbContext.SaveChanges();
                }
                if (!dbContext.CatalogTypes.Any())
                {
                    dbContext.CatalogTypes.AddRange(GetPreconfiguredCatalogTypes);
                    dbContext.SaveChanges();
                }
            }
        });
        logger.LogInformation("{ContextType} Migration Seed End", nameof(CatalogDbContext));
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands
        => new List<CatalogBrand>()
        {
                 new CatalogBrand() { Brand = "Azure"},
                 new CatalogBrand() { Brand = ".NET" },
                 new CatalogBrand() { Brand = "Visual Studio" },
                 new CatalogBrand() { Brand = "SQL Server" }
        };
    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes
        => new List<CatalogType>()
        {
                 new CatalogType() { Type = "Mug"},
                 new CatalogType() { Type = "T-Shirt" },
                 new CatalogType() { Type = "Backpack" },
                 new CatalogType() { Type = "USB Memory Stick" }
        };

    private static RetryPolicy CreatePolicy(ILogger<CatalogDbContext> logger, int retries = 3)
        => Policy.Handle<SqlException>()
            .WaitAndRetry(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, context) =>
                {
                    logger.LogWarning(exception, "[Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", exception.GetType().Name, exception.Message, retry, retries);
                }

         );
}
