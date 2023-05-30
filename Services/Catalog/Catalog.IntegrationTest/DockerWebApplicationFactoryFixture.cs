namespace Catalog.IntegrationTest;

public class DockerWebApplicationFactoryFixture : WebApplicationFactory<Program>, 
    IAsyncLifetime
{
    private MsSqlContainer _builder;
    private static RabbitMqContainer _rabbitMqContainer;
    private string _hostName = "testrabbit";
    private string _username = "guest";
    private string _password = "guest";
    private string _queueName = "TestCatalog";
    private int port;
    public DockerWebApplicationFactoryFixture()
    {
        port = Random.Shared.Next(10000, 50000);
        _builder = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Sarma123#")
            .Build();
        
        _rabbitMqContainer = new RabbitMqBuilder().WithName(_hostName)
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
        await _builder.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _builder.GetConnectionString();
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
        await _builder.StopAsync();
        await _rabbitMqContainer.StopAsync();
        
    }
}