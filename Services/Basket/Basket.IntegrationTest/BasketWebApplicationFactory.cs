namespace Basket.IntegrationTest;

public class BasketWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisBuilder;
    private readonly RabbitMqContainer _rabbitMqContainer;

    private string _redisHostName = "testbasketrredis" + Guid.NewGuid().ToString();

    private string _rabbitHostName = "testrabbit" + Guid.NewGuid().ToString();
    private string _username = "guest";
    private string _password = "guest";
    private string _queueName = "TestCatalog";

    private int port;

    public BasketWebApplicationFactory()
    {
        port = Random.Shared.Next(100,1000);

        _redisBuilder = new RedisBuilder()
            .WithImage("redis").WithName(_redisHostName)
            .WithPortBinding(port+1,6379)
            .Build();

        _rabbitMqContainer = new RabbitMqBuilder().WithName(_rabbitHostName)
             .WithHostname(_rabbitHostName)
             .WithImage("rabbitmq:3-management-alpine")
             .WithUsername(_username)
             .WithPassword(_password)
             .WithPortBinding(port,15672)
             .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
             .Build();
    }
    public async Task InitializeAsync()
    {
        await _redisBuilder.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var redisConnectionString = _redisBuilder.GetConnectionString();
        builder.ConfigureServices(services =>
        {
            services.RedisConnectionMultyplexer(redisConnectionString);

            services.ConfigurationEventBus(connectionUri: _rabbitMqContainer.GetConnectionString())
                .RegisterEventBusRabbitMQ(_queueName);            
        });

    }

    public async Task DisposeAsync()
    {
        await _rabbitMqContainer.StopAsync();
        await _redisBuilder.StopAsync();
    }
}
