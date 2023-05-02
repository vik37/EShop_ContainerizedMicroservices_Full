namespace EShop.Basket.API;

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
}
