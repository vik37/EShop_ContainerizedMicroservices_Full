namespace Basket.IntegrationTest.TestContainers.Abstractions;

public interface ITestContainersConfig<T> where T : class
{
    T? TestContainer { get; set; }
    void TestContainerBuild(int port);
}