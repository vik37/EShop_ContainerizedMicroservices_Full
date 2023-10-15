using EShop.Orders.API.Application.Queries.OrderQueries;

namespace EShop.Orders.IntegrationTest;

public class OrderWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly ITestContainersConfigWithCustomConnectionStrig<MsSqlContainer> _mssqlContainer
        = new MssqlTestContainerConfig();
    private readonly ITestContainersConfigWithConnectionPort<RabbitMqContainer> _rabbitMqContainer = new RabbitMQTestContainerConfig();

    private readonly string _rabbitHostName;

    private string _mssqlConnectionString = string.Empty;
    
    private readonly int _port;
    

    public OrderWebApplicationFactory()
    {
        _port = Random.Shared.Next(1000,9990);
        
        _mssqlContainer.TestContainerBuild(_port+3);
        _rabbitMqContainer.TestContainerBuild(_port-3);

        _rabbitHostName = _rabbitMqContainer.TestContainer!.Hostname;
    }

    public async Task InitializeAsync()
    {
        await _mssqlContainer.TestContainer!.StartAsync();
        await _rabbitMqContainer.TestContainer!.StartAsync();       
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {      
        builder.ConfigureServices(services =>
        {
            _mssqlConnectionString = _mssqlContainer.ConnectionString;

            services.RemoveAll(typeof(DbContextOptions<OrderingContext>));
            services.RemoveAll(typeof(DbContextOptions<IntegrationEventLogDbContext>));

            services.DatabaseConfiguration(_mssqlConnectionString);

            services.RemoveAll(typeof(IRabbitMQPersistentConnection));

            var eventBusSettings = new EventBusSettings(_rabbitHostName, RabbitMQTestContainerConfig.SubscriptionClient,
                       RabbitMQTestContainerConfig.Username, RabbitMQTestContainerConfig.Password);

            services.ConfigurationEventBus(eventBusSettings, port: _rabbitMqContainer.ConnectionPort);
            services.RegisterEventBusRabbitMQ(eventBusSettings);           

            services.RemoveAll(typeof(IOrderQuery));

            services.AddScoped<IOrderQuery, OrderQuery>(o => new OrderQuery(_mssqlConnectionString));         
        });      
    }

    public new Task DisposeAsync()
    {       
        _mssqlContainer.TestContainer!.StopAsync().Wait();
        _rabbitMqContainer.TestContainer!.StopAsync().Wait();

        return Task.CompletedTask;
    }
}
