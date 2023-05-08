namespace EShop.Web.MVC;

public sealed class Application
{
    public string AppNamespace { get; private set; }
    public string ApplicationName { get; private set; }
    private Application()
    {
        this.AppNamespace = typeof(Application).Assembly.GetName().Name ?? null;
        if (this.AppNamespace is not null)
            this.ApplicationName = this.AppNamespace.Substring(this.AppNamespace.LastIndexOf('.', this.AppNamespace.LastIndexOf('.') - 1) + 1);
    }

    public static Application GetApplication() => new Application();

    public string DockerInternalCatalog(IConfiguration configuration) => configuration["DockerInternalCatalog"];

    public string CatalogDockerURL(IConfiguration configuration) => configuration["CatalogAPIDocker"];

    public string BasketDockerURL(IConfiguration configuration) => configuration["BasketAPIDocker"];

    public string CatalogLocalKestrelURL(IConfiguration configuration) => configuration["HttpClient:CatalogAPILocal"];

    public string BasketLocalKestrelURL(IConfiguration configuration) => configuration["HttpClient:BasketAPILocal"];
}
