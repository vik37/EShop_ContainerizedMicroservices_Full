using EShop.Catalog.API.Entities;
using EShop.Catalog.API.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace EShop.Catalog.API.Infrastructure;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
       base(options)
    { }

    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;
    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;
    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}

public class CatalogContext : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer("Data Source=DESKTOP-1IQ2NGF\\SQLEXPRESS;Initial Catalog=CatalogDb;Integrated Security=true",
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        return new CatalogDbContext(optionsBuilder.Options);
    }
}
