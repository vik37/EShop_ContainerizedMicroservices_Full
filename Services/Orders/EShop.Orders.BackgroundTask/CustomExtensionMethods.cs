namespace EShop.Orders.BackgroundTask;

public static class CustomExtensionMethods
{
    public static IServiceCollection BindConfigurationOptions(this IServiceCollection services,IConfiguration configuration)
       => services.AddSingleton<BackgroundTaskOptionSettings>()
                   .Configure<BackgroundTaskOptionSettings>(configuration.GetRequiredSection(nameof(BackgroundTaskOptionSettings)))
                    .Configure<BackgroundTaskOptionSettings>(opt =>
                    {
                        opt.DbContext = configuration["OrderingDbConnection"];
                    });
}