namespace Catalog.IntegrationTest;

public class CatalogWebApplicationFactory : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private string _mssqlConnectionString = string.Empty;
    private readonly int _port;
    private readonly string _rabbitHostName;
    private readonly ITestContainersConfigWithCustomConnectionStrig<MsSqlContainer> _mssqlConntainer = new MssqlTestContainerConfig();
    private readonly ITestContainersConfigWithConnectionPort<RabbitMqContainer> _rabbitMqConntainer = new RabbitMQTestContainerConfig();

    public CatalogWebApplicationFactory()
    {
        _port = Random.Shared.Next(1000, 9990);
        _mssqlConntainer.TestContainerBuild(_port - 1);
        _rabbitMqConntainer.TestContainerBuild(_port + 1);
        _rabbitHostName = _rabbitMqConntainer.TestContainer.Hostname;
    }

    public async Task InitializeAsync()
    {
        await _mssqlConntainer.TestContainer.StartAsync();
        await _rabbitMqConntainer.TestContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<CatalogDbContext>));

            services.RemoveAll(typeof(DbContextOptions<IntegrationEventLogDbContext>));

            _mssqlConnectionString = _mssqlConntainer.ConnectionString;

            services.DatabaseConfiguration(_mssqlConnectionString);

            var eventBusSettings = new EventBusSettings(_rabbitHostName, RabbitMQTestContainerConfig.SubscriptionClient,
                        RabbitMQTestContainerConfig.Username, RabbitMQTestContainerConfig.Password);

            services.RemoveAll(typeof(IRabbitMQPersistentConnection));

            services.ConfigurationEventBus(eventBusSettings, port: _rabbitMqConntainer.ConnectionPort);
            services.RegisterEventBusRabbitMQ(eventBusSettings);
        });
    }

    public new Task DisposeAsync()
    {
        _mssqlConntainer.TestContainer.StopAsync().Wait();
        _rabbitMqConntainer.TestContainer.StopAsync().Wait();

        return Task.CompletedTask;
    }
}