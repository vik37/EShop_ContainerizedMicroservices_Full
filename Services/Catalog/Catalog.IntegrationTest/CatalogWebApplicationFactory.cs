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
        _mssqlConnectionString = MssqlTestContainerConfig.TestContainerMssqlBuilder(_port + 1);
        _rabbitHostName = RabbitMQTestContainerConfig.TestContainerRabbitMQBuilder(_port - 1);       
    }

    public async Task InitializeAsync()
    {
        await MssqlTestContainerConfig.MsSqlBuilder.StartAsync();
        await RabbitMQTestContainerConfig.RabbitMqContainer.StartAsync();
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

            services.AddDbContext<CatalogDbContext>(o => 
                o.UseSqlServer(_mssqlConnectionString));

            services.AddDbContext<IntegrationEventLogDbContext>(o =>
                o.UseSqlServer(_mssqlConnectionString));
            
        });
    }

    public async Task DisposeAsync()
    {
        await MssqlTestContainerConfig.MsSqlBuilder.StopAsync();
        await RabbitMQTestContainerConfig.RabbitMqContainer.StopAsync();
    }
}