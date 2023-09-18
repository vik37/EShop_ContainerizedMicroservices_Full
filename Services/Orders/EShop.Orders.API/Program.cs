
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;

var orderApplication = OrderApplication.GetApplication();

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

services.AddControllers()
    .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

var eventBusSettings = new EventBusSettings(configuration["RabbitMQConnection"]!, configuration["SubscriptionClientName"]!,
                        configuration["EventBusRabbitMQUsername"]!, configuration["EventBusRabbitMQPassword"]!, orderApplication.RabbitMQRetry(configuration));

services.AddMemoryCache(opt => opt.ExpirationScanFrequency = TimeSpan.FromHours(1));


services.SwaggerConfigurations()
                .ApiVersioning()
                .CorsConfiguration()
                .DatabaseConfiguration(configuration.GetValue<string>("OrderingDb"))
                .ConfigurationEventBus(eventBusSettings)
                .RegisterEventBusRabbitMQ(eventBusSettings);

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidateBehavior<,>));
});

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


services.AddScoped<IBuyerRepository, BuyerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IRequestManager, RequestManager>();


services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();
services.AddSingleton<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
services.AddSingleton<IValidator<IdentifiedCommand<CreateOrderCommand, bool>>, IdentifiedCommandValidator>();
services.AddSingleton<IValidator<ShipOrderCommand>, ShipOrderCommandValidator>();

services.AddTransient<IOrderQuery, OrderQuery>(o => new OrderQuery(configuration.GetValue<string>("OrderingDb")));



services.AddTransient<IIntegrationEventHandler<GracedPeriodConfirmedIntegrationEvent>, GracePeriodConfirmedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>, OrderPaymentFailedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>, OrderPaymentSucceededIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>, OrderStockConfirmedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>, OrderStockRejectedIntegrationEventHandler>();


try
{
    var app = builder.Build();
    Log.Information("Application {ApplicationName} Starting", orderApplication.ApplicationName);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        });
    }

    app.UseSerilogRequestLogging();

    app.UseCors("Order Cors");

    app.UseAuthorization();

    app.MapControllers();

    var eventBus = app.Services.GetRequiredService<IEventBus>();

    eventBus.Subscribe<GracedPeriodConfirmedIntegrationEvent, IIntegrationEventHandler<GracedPeriodConfirmedIntegrationEvent>>();
    eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>>();
    eventBus.Subscribe<OrderStockRejectedIntegrationEvent, IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>>();
    eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
    eventBus.Subscribe<OrderPaymentSucceededIntegrationEvent, IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>>();

    app.MigrateDbContext();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application {Application} Failes", orderApplication.AppNamespace);
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }