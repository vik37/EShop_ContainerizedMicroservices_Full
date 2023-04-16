﻿namespace EShop.Catalog.API.Infrastructure;

public class CatalogContextSeed
{
    public void Seed(CatalogDbContext dbContext, ILogger<CatalogDbContext> logger)
    {        
        var policy = CreatePolicy(logger);
        policy.Execute(() =>
        {
            if (dbContext != null)
            {
                logger.LogInformation("{ContextType} Migration Seed Start", nameof(CatalogDbContext));
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
                if (!dbContext.CatalogItems.Any())
                {
                    dbContext.CatalogItems.AddRange(GetPreconfiguredCatalogItems);
                    dbContext.SaveChanges();
                }
                logger.LogInformation("{ContextType} Migration Seed End", nameof(CatalogDbContext));
            }
        });       
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands
        => new List<CatalogBrand>()
        {
                 new CatalogBrand() { Brand = "Azure"},
                 new CatalogBrand() { Brand = ".NET" },
                 new CatalogBrand() { Brand = "Visual Studio" },
                 new CatalogBrand() { Brand = "SQL Server" },
                 new CatalogBrand() { Brand = "Other" }
        };
    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes
        => new List<CatalogType>()
        {
                 new CatalogType() { Type = "Mug"},
                 new CatalogType() { Type = "T-Shirt" },
                 new CatalogType() { Type = "Backpack" },
                 new CatalogType() { Type = "USB Memory Stick" }
        };

    private IEnumerable<CatalogItem> GetPreconfiguredCatalogItems=>
        new List<CatalogItem>()
        {
            new() { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png" },
            new() { CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, PictureFileName = "2.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png" },
            new() { CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png" },
            new() { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 100, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png" },
            new() { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png" },
            new() { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png" },
            new() { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png" },
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
