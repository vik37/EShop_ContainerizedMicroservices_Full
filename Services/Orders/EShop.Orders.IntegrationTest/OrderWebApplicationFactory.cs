namespace EShop.Orders.IntegrationTest;

public class OrderWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _mssqlConnectionString;
    private readonly int _port;
    private readonly string _rabbitHostName;

    public OrderWebApplicationFactory()
    {
        _port = Random.Shared.Next(1000,9990);
        _rabbitHostName = RabbitMQTestContainerConfig.TestContainerRabbitMQBuilder(_port - 1);
        _mssqlConnectionString = MssqlTestContainerConfig.TestContainerMssqlBuilder(_port+1);
    }

    public async Task InitializeAsync()
    {     
        if(RabbitMQTestContainerConfig.RabbitMqContainer is not null)
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

            services.RemoveAll(typeof(DbContextOptions<OrderContext>));
            services.RemoveAll(typeof(DbContextOptions<IntegrationEventLogDbContext>));

            services.DatabaseConfiguration(_mssqlConnectionString);

            services.RemoveAll(typeof(IOrderQuery));

            services.AddScoped<IOrderQuery, OrderQuery>(o => new OrderQuery(_mssqlConnectionString));
           
        });
        
    }

    public async Task DisposeAsync()
    {       
        await RabbitMQTestContainerConfig.RabbitMqContainer!.StopAsync();
        await MssqlTestContainerConfig.MsSqlBuilder!.StopAsync();
    }
}
