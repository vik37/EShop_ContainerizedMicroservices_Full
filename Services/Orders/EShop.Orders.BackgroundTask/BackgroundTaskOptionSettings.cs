namespace EShop.Orders.BackgroundTask;

public class BackgroundTaskOptionSettings
{
    public string EventBusConnection { get; set; }
    public int GracePeriodTime { get; set; }
    public int CheckUpdateTime { get; set; }
    public string DbContext { get; set; }
}