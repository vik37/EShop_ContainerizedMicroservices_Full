namespace Catalog.IntegrationTest;

public class CatalogWebApplicationFactory : WebApplicationFactory<Program>, 
    IAsyncLifetime
{
    private readonly string _mssqlConnectionString;
    private readonly int _port;
    private readonly string _rabbitHostName;

    public CatalogWebApplicationFactory()
    {
        _port = Random.Shared.Next(1000, 9990);
        _rabbitHostName = RabbitMQTestContainerConfig.TestContainerRabbitMQBuilder(_port - 1);
        _mssqlConnectionString = MssqlTestContainerConfig.TestContainerMssqlBuilder(_port + 1);
    }

    public async Task InitializeAsync()
    {
        if (RabbitMQTestContainerConfig.RabbitMqContainer is not null)
        {
            await RabbitMQTestContainerConfig.RabbitMqContainer.StartAsync();
        }
        if (MssqlTestContainerConfig.MsSqlBuilder is not null)
        {
            await MssqlTestContainerConfig.MsSqlBuilder.StartAsync();
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IRabbitMQPersistentConnection));

            services.ConfigurationEventBus(rabbitConnection: _rabbitHostName, rabbitUsername: RabbitMQTestContainerConfig.Username,
                                            rabbitPassword: RabbitMQTestContainerConfig.Password, port: RabbitMQTestContainerConfig.ConnectionPort.ToString());
            services.RegisterEventBusRabbitMQ(RabbitMQTestContainerConfig.SubscriptionClient);

            services.RemoveAll(typeof(DbContextOptions<CatalogDbContext>));

            services.RemoveAll(typeof(DbContextOptions<IntegrationEventLogDbContext>));

            services.DatabaseConfiguration(_mssqlConnectionString);
        });
    }

    public async Task DisposeAsync()
    {
        await RabbitMQTestContainerConfig.RabbitMqContainer!.StopAsync();
        await MssqlTestContainerConfig.MsSqlBuilder!.StopAsync();
    }
}