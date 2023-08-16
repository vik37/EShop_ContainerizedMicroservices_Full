namespace Catalog.IntegrationTest.TestContainers;

public class RabbitMQTestContainerConfig : ITestContainersConfigWithConnectionPort<RabbitMqContainer>
{
    private const string HostName = "rabbitmqcatalogtest";
    private string _name = $"{HostName}_{Guid.NewGuid().ToString()}";

    public const string Username = "guest";
    public const string Password = "guest";
    public const string SubscriptionClient = "TestCatalog";
    public int ConnectionPort => _connectionPort;
    private int _connectionPort;

    public RabbitMqContainer TestContainer { get; set; }

    public void TestContainerBuild(int port)
    {
        _connectionPort = Random.Shared.Next(10000, 90000);
        TestContainer = new RabbitMqBuilder()
                            .WithName(_name)
                            .WithHostname(HostName)
                            .WithImage("rabbitmq:3-management-alpine")
                            .WithPortBinding(port, 15672)
                            .WithPortBinding(_connectionPort, 5672)
                            .WithUsername(Username)
                            .WithPassword(Password)
                            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                            .Build();
    }
}