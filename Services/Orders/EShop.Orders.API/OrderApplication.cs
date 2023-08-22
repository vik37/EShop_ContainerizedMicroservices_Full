namespace EShop.Orders.API;

public sealed class OrderApplication
{
    public string? AppNamespace { get; private set; }
    public string? ApplicationName { get; private set; }
    private OrderApplication()
    {
        this.AppNamespace = typeof(OrderApplication).Assembly.GetName().Name ?? null;
        if (this.AppNamespace is not null)
            this.ApplicationName = this.AppNamespace.Substring(this.AppNamespace.LastIndexOf('.', this.AppNamespace.LastIndexOf('.') - 1) + 1);
    }

    public static OrderApplication GetApplication() => new();

    public int RabbitMQRetry(IConfiguration configuration) => int.Parse(configuration["EventBusRetry"]??
        throw new Exception("Connection String for RabbitMQ Retry is empty or it's not a number"));

    public string LocalMSQLConnectionString(IConfiguration configuration) => configuration["OrderingDbConnection"]
        ?? throw new Exception("Wrong Database Connection! Please check the configuration file");

    public string DockerMSQLConnectionString(IConfiguration configuration) => configuration["OrderingDockerDbConnectionString"]
        ?? throw new Exception("Wrong Database Connection! Please check the configuration file");

}
