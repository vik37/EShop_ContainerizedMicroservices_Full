namespace Catalog.IntegrationTest;

public class CatalogWebApplicationFactory : WebApplicationFactory<Program>, 
    IAsyncLifetime
{
    private MsSqlContainer _mssqlContainer;
    private RabbitMqContainer _rabbitMqContainer;

    private string _hostName = "testcatalograbbit";
    private string _username = "guest";
    private string _password = "guest";
    private string _queueName = "TestCatalog";

    private int port;

    public CatalogWebApplicationFactory()
    {
        port = Random.Shared.Next(10000, 50000);
        _mssqlContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("TestMSQL123#")
            .Build();
        
        _rabbitMqContainer = new RabbitMqBuilder().WithName(_hostName+Guid.NewGuid().ToString())
            .WithHostname(_hostName)
            .WithImage("rabbitmq:3-management-alpine")
            .WithPortBinding(port,15672)
            .WithUsername(_username)
            .WithPassword(_password)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();
    }
    public async Task InitializeAsync()
    {
        await _mssqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _mssqlContainer.GetConnectionString();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<CatalogDbContext>));

            services.RemoveAll(typeof(DbContextOptions<IntegrationEventLogDbContext>));

            services.AddDbContext<CatalogDbContext>(opt =>
                opt.UseSqlServer(connectionString));

            services.AddDbContext<IntegrationEventLogDbContext>(opt =>
                opt.UseSqlServer(connectionString));

            services.ConfigurationEventBus(connectionUri: _rabbitMqContainer.GetConnectionString())
                .RegisterEventBusRabbitMQ(_queueName);
        });
        builder.UseEnvironment("Development");
    }

    public async Task DisposeAsync()
    {
        await _mssqlContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
        
    }
}