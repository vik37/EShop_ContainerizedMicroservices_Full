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
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        configuration.SetBasePath(Directory.GetCurrentDirectory());
        configuration.AddJsonFile("appsettings.json");

        var connectionString = builder.Configuration["LocalDbConnectionString"];
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer(connectionString,
                db => db.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        return new CatalogDbContext(optionsBuilder.Options);
    }
}
