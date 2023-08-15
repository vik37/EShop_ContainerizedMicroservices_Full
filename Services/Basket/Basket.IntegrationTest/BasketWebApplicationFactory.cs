namespace Basket.IntegrationTest;

public class BasketWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime, IDisposable
{
    private readonly int _port;
    private readonly string _rabbitHostName;

    private string _redisConnectionString = string.Empty;


    public BasketWebApplicationFactory()
    {
        _port = Random.Shared.Next(900, 9990);

       RedisTestContainerConfig.TestContainerRedisBuilder(_port + 2);
        
        _rabbitHostName = RabbitMQTestContainerConfig.TestContainerRabbitMQBuilder(_port - 2);
    }
    public async Task InitializeAsync()
    {
        await RedisTestContainerConfig.RedisContainer!.StartAsync();
        await RabbitMQTestContainerConfig.RabbitMqContainer!.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _redisConnectionString = RedisTestContainerConfig.RedisContainer!.GetConnectionString();
            services.RedisConnectionMultyplexer(_redisConnectionString);

            services.RemoveAll(typeof(IRabbitMQPersistentConnection));

            services.ConfigurationEventBus(rabbitConnection: _rabbitHostName, rabbitUsername: RabbitMQTestContainerConfig.Username,
                                            rabbitPassword: RabbitMQTestContainerConfig.Password, port: RabbitMQTestContainerConfig.ConnectionPort.ToString());
            services.RegisterEventBusRabbitMQ(RabbitMQTestContainerConfig.SubscriptionClient);
        });

    }

    public async Task DisposeAsync()
    {
        await RedisTestContainerConfig.RedisContainer!.StopAsync(default);
        await RabbitMQTestContainerConfig.RabbitMqContainer!.StopAsync(default);
    }
}
