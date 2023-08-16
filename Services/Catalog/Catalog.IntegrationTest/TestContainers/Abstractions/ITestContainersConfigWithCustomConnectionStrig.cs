namespace Catalog.IntegrationTest.TestContainers.Abstractions;

public interface ITestContainersConfigWithCustomConnectionStrig<T> : ITestContainersConfig<T>
    where T : class
{
    string ConnectionString { get; }
}