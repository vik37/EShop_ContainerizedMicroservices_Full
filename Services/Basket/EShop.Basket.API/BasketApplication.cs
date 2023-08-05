namespace EShop.Basket.API;

public sealed class BasketApplication
{
    public string AppNamespace { get; private set; }
    public string ApplicationName { get; private set; }
    private BasketApplication()
    {
        this.AppNamespace = typeof(BasketApplication).Assembly.GetName().Name ?? null;
        if (this.AppNamespace is not null)
            this.ApplicationName = this.AppNamespace.Substring(this.AppNamespace.LastIndexOf('.', this.AppNamespace.LastIndexOf('.') - 1) + 1);
    }

    public static BasketApplication GetApplication() => new BasketApplication();

    public int RabbitMQRetry(IConfiguration configuration) => int.Parse(configuration["EventBusRetry"]);
}
