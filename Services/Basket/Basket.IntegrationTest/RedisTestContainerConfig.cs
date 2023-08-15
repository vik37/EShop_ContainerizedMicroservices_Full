namespace Basket.IntegrationTest;

public class RedisTestContainerConfig
{
    private const string HostName = "redistestname";
    public static RedisContainer? RedisContainer = null;

    public static void TestContainerRedisBuilder(int port)
    {
        RedisContainer = new RedisBuilder()
                            .WithName(HostName+"_"+Guid.NewGuid().ToString())
                            .WithHostname(HostName)
                            .WithImage("redis")
                            .WithPortBinding(port, 6379)
                            .Build();
    }
}