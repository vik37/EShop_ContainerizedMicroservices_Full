
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

IConfiguration configuration = builder.Configuration;
var services = builder.Services;
// Add services to the container.

var application = BasketApplication.GetApplication();

services.AddControllers()
    .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.SwaggerConfigurations()
        .CorsConfiguration()
        .ApiVersioning()
        .RedisConnectionMultyplexer(configuration["RedisConnectionString"])
        .RegisterEventBusRabbitMQ(subscriptionClientName: configuration["SubscriptionClientName"], application.RabbitMQRetry(configuration))
        .ConfigurationEventBus(configuration,retryConnection: application.RabbitMQRetry(configuration));

services.AddTransient<IBasketRepository, BasketRepository>();



// Configure the HTTP request pipeline.
try
{
    var app = builder.Build();
    Log.Information("Application {ApplicationName} Starting", application.ApplicationName);
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        });
    }

    app.UseSerilogRequestLogging();

    app.UseCors("Basket Cors");

    app.UseAuthorization();

    app.MapControllers();

    ConfigurationEvents(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application {Application} Failes", application.AppNamespace);
}
finally
{
    Log.CloseAndFlush();
}

void ConfigurationEvents(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
    eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
}

public partial class Program { }
