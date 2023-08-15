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

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.SwaggerConfigurations()
                .ApiVersioning()
                .DatabaseConfiguration(configuration["OrderingDockerDbConnectionString"] ?? string.Empty)
                .ConfigurationEventBus(rabbitConnection: configuration["RabbitMQConnection"]!, rabbitUsername: configuration["EventBusRabbitMQUsername"]!,
                                        rabbitPassword: configuration["EventBusRabbitMQPassword"]!, retryConnection: orderApplication.RabbitMQRetry(configuration))
                .RegisterEventBusRabbitMQ(configuration["SubscriptionClientName"] ?? string.Empty, orderApplication.RabbitMQRetry(configuration));

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidateBehavior<,>));
});

services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();
services.AddSingleton<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
services.AddSingleton<IValidator<IdentifiedCommand<CreateOrderCommand, bool>>, IdentifiedCommandValidator>();
services.AddSingleton<IValidator<ShipOrderCommand>, ShipOrderCommandValidator>();

services.AddScoped<IOrderQuery, OrderQuery>(o => new OrderQuery(configuration["OrderingDockerDbConnectionString"] ?? string.Empty));
services.AddScoped<IBuyerRepository, BuyerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IRequestManager, RequestManager>();

services.AddTransient<IIntegrationEventHandler<GracedPeriodConfirmedIntegrationEvent>, GracePeriodConfirmedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>, OrderPaymentFailedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>, OrderPaymentSucceededIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>, OrderStockConfirmedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>, OrderStockRejectedIntegrationEventHandler>();
services.AddTransient<IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>, UserCheckoutAcceptedIntegrationEventHandler>();

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

    app.UseAuthorization();

    app.MapControllers();

    var eventBus = app.Services.GetRequiredService<IEventBus>();

    eventBus.Subscribe<UserCheckoutAcceptedIntegrationEvent, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>>();
    eventBus.Subscribe<GracedPeriodConfirmedIntegrationEvent, IIntegrationEventHandler<GracedPeriodConfirmedIntegrationEvent>>();
    eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>>();
    eventBus.Subscribe<OrderStockRejectedIntegrationEvent, IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>>();
    eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
    eventBus.Subscribe<OrderPaymentSucceededIntegrationEvent, IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>>();

    app.MigrateDbContext();

    app.Run();
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