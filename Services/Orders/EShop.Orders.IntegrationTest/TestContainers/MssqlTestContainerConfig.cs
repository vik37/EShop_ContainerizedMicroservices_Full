namespace EShop.Orders.IntegrationTest.TestContainers;

public class MssqlTestContainerConfig : ITestContainersConfigWithCustomConnectionStrig<MsSqlContainer>
{
    private Dictionary<string, string>? _mSQlConfigCollection = null;

    public MsSqlContainer? TestContainer { get; set; }

    public string ConnectionString { get { return _connectionString; } }

    private string _connectionString = string.Empty;

    public void TestContainerBuild(int port)
    {
        _connectionString = MssqlConnectionStringBuilder(port);

        TestContainer = new MsSqlBuilder()
                            .WithName(_mSQlConfigCollection!["Name"]+"_"+Guid.NewGuid().ToString())
                            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                            .WithPortBinding(port, 1433)
                            .WithPassword(_mSQlConfigCollection["Password"])
                            .Build();

    }

    private string MssqlConnectionStringBuilder(int port)
    {
        StringBuilder sb = new();

        _mSQlConfigCollection = new Dictionary<string, string>
        {
            {"Name","ordertestdb" },{"Server",$"host.docker.internal,{port}"}, {"Database","EShop_TestOrderDb"},{"User Id","sa"},{"Password","tstOrder123#" }
        };

        foreach (string key in _mSQlConfigCollection.Keys)
        {
            if (key != "Name")
                sb.Append($"{key}={_mSQlConfigCollection[key]};");
        }

        return sb.ToString().Trim();
    }
}