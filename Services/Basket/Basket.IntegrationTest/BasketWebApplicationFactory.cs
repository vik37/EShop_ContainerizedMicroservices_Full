using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using EShop.Basket.API;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Basket.IntegrationTest;

public class BasketWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisBuilder;
    private readonly RabbitMqContainer _rabbitMqContainer;

    private string _hostName = "testrabbit";
    private string _username = "guest";
    private string _password = "guest";
    private string _queueName = "TestCatalog";
    private int port;

    public BasketWebApplicationFactory()
    {
        _redisBuilder = new RedisBuilder()
            .WithImage("redis").WithName("testbasketdata")
            .WithPortBinding(port,6379).Build();

        _rabbitMqContainer = new RabbitMqBuilder().WithName(_hostName)
             .WithHostname(_hostName)
             .WithImage("rabbitmq:3-management-alpine")
             .WithPortBinding(port, 15672)
             .WithUsername(_username)
             .WithPassword(_password)
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
        await _redisBuilder.StopAsync();
        await _rabbitMqContainer.StopAsync();
    }
}
