namespace Catalog.IntegrationTest.TestContainers.Abstractions;

public interface ITestContainersConfigWithConnectionPort<T> : ITestContainersConfig<T>
    where T : class
{
    int ConnectionPort { get; }
}