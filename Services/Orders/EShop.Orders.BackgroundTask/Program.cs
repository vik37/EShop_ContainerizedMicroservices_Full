IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.BindConfigurationOptions(context.Configuration);
        services.AddHostedService<GracePeriodManagerService>();
    })
    .Build();

await host.RunAsync();
