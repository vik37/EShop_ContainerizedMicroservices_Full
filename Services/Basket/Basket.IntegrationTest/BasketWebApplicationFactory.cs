namespace Basket.IntegrationTest;


public class BasketWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly ITestContainersConfig<RedisContainer> _redisContainer = new RedisTestContainerConfig();
    private readonly ITestContainersConfigWithConnectionPort<RabbitMqContainer> _rabbitMQContainer = new RabbitMQTestContainerConfig();

    private readonly int _port;

    private string _redisConnectionString = string.Empty;
    private string _rabbitHostName = string.Empty; 

    public BasketWebApplicationFactory()
    {
        _port = Random.Shared.Next(900, 9990);

        _redisContainer.TestContainerBuild(_port - 2);
        _rabbitMQContainer.TestContainerBuild(_port + 2);     
    }
    public async Task InitializeAsync()
    {
        await _redisContainer.TestContainer!.StartAsync();
        await _rabbitMQContainer.TestContainer!.StartAsync();       
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _redisConnectionString = _redisContainer.TestContainer!.GetConnectionString();
            _rabbitHostName = _redisContainer.TestContainer!.Hostname;

            services.RemoveAll(typeof(IConnectionMultiplexer));

            services.RedisConnectionMultyplexer(_redisConnectionString);

            services.RemoveAll(typeof(IRabbitMQPersistentConnection));

            services.ConfigurationEventBus(rabbitConnection: _rabbitHostName, rabbitUsername: RabbitMQTestContainerConfig.Username,
                                            rabbitPassword: RabbitMQTestContainerConfig.Password, port: _rabbitMQContainer.ConnectionPort.ToString());
            services.RegisterEventBusRabbitMQ(RabbitMQTestContainerConfig.SubscriptionClient);
        });

    }

    public new Task DisposeAsync()
    {
        _redisContainer.TestContainer!.StopAsync().Wait();
        _rabbitMQContainer.TestContainer!.StopAsync().Wait();
        
        return Task.CompletedTask;
    }
}

[CollectionDefinition("BasketApplication Collection")]
public class BasketApplicationCollection : ICollectionFixture<BasketWebApplicationFactory> { }