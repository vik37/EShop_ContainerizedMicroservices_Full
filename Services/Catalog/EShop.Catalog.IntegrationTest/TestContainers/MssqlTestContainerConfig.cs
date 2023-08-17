namespace Catalog.IntegrationTest.TestContainers;

public class MssqlTestContainerConfig : ITestContainersConfigWithCustomConnectionStrig<MsSqlContainer>
{
    private Dictionary<string, string> _mSQlConfigCollection = null;

    public string ConnectionString { get { return _connectionString; } }

    public MsSqlContainer TestContainer { get; set; }

    private string _connectionString;

    public void TestContainerBuild(int port)
    {
        _connectionString = MssqlConnectionStringBuilder(port);

        TestContainer = new MsSqlBuilder()
                            .WithName(_mSQlConfigCollection["Name"] + "_" + Guid.NewGuid().ToString())
                            .WithHostname(_mSQlConfigCollection["Name"])
                            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                            .WithPortBinding(port, 1433)
                            .WithPassword(_mSQlConfigCollection["Password"])
                            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                            .Build();


    }

    private string MssqlConnectionStringBuilder(int port)
    {
        StringBuilder sb = new();

        _mSQlConfigCollection = new Dictionary<string, string>
        {
            {"Name","catalogtestdb" },{"Server",$"127.0.0.1,{port}"}, {"Database","EShop_TestCatalogDb"},{"User Id","sa"},{"Password","tstCatalog123#" }
        };

        foreach (string key in _mSQlConfigCollection.Keys)
        {
            if (key != "Name")
                sb.Append($"{key}={_mSQlConfigCollection[key]};");
        }
        sb.Append("TrustServerCertificate=True");

        return sb.ToString();
    }
}