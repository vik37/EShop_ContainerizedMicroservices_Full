
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

IConfiguration configuration = builder.Configuration;
var services = builder.Services;
// Add services to the container.


services.AddControllers()
    .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.SwaggerConfigurations()
        .CorsConfiguration()
        .RedisConnectionMultyplexer(configuration)
        .RegisterEventBusRabbitMQ(configuration)
        .ConfigurationEventBus(configuration);

services.AddTransient<IBasketRepository, BasketRepository>();



// Configure the HTTP request pipeline.
try
{
    var app = builder.Build();
    Log.Information("Application {ApplicationName} Starting", Application.GetApplication().ApplicationName);
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
    Log.Fatal(ex, "Application {Application} Failes", Application.GetApplication().AppNamespace);
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
