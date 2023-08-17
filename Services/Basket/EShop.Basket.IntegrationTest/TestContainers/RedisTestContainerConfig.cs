namespace Basket.IntegrationTest.TestContainers;

public class RedisTestContainerConfig : ITestContainersConfig<RedisContainer>
{
    private const string HostName = "redisbaskettest";

    private string _name = $"{HostName}_{Guid.NewGuid().ToString()}"; 
        
    public RedisContainer? TestContainer { get; set; }

    public void TestContainerBuild(int port)
    {
        TestContainer = new RedisBuilder()
                            .WithName(_name)
                            .WithHostname(HostName)
                            .WithImage("redis")
                            .WithPortBinding(port, 6379)
                            .Build();
    }
}