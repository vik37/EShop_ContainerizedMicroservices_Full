namespace EShop.Catalog.API;

public sealed class CatalogApplication
{
    public string? AppNamespace { get; private set; }
    public string? ApplicationName { get; private set; }
    private CatalogApplication()
    {
        this.AppNamespace = typeof(CatalogApplication).Assembly.GetName().Name ?? null;
        if (this.AppNamespace is not null)
            this.ApplicationName = this.AppNamespace.Substring(this.AppNamespace.LastIndexOf('.', this.AppNamespace.LastIndexOf('.') - 1) + 1);
    }


    public static CatalogApplication GetApplication() => new CatalogApplication();

    public string LocalMSQLConnectionString(IConfiguration configuration) => configuration["LocalDbConnectionString"];

    public string DockerMSQLConnectionString(IConfiguration configuration) => configuration["CatalogDockerDbConnectionString"];

    public int RabbitMQRetry(IConfiguration configuration) => int.Parse(configuration["EventBusRetry"]);
}
