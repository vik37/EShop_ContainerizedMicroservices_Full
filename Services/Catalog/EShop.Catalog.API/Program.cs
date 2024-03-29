Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "Files/Images"
});

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<CatalogOptionSettings>(configuration)
                .AddOptions<CatalogOptionSettings>()
                .ValidateOnStart();

// Add services to the container.
var services = builder.Services;

services.TryAddSingleton<IValidateOptions<CatalogOptionSettings>, CatalogOptionsSettingsValidation>();

var catalogApplication = CatalogApplication.GetApplication();

var eventBusSettings = new EventBusSettings(configuration["RabbitMQConnection"], configuration["SubscriptionClientName"],
                        configuration["EventBusRabbitMQUsername"], configuration["EventBusRabbitMQPassword"], catalogApplication.RabbitMQRetry(configuration));


//***** Custom Extension Methods - Application Configurations *****\\\

services.SwaggerConfigurations()
        .DatabaseConfiguration(catalogApplication.DockerMSQLConnectionString(configuration))
        .CorsConfiguration()
        .ApiVersioning()
        .ConfigurationEventBus(eventBusSettings)
        .RegisterEventBusRabbitMQ(eventBusSettings);
        

services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddControllers()
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

try
{
    var app = builder.Build();

    Log.Information("Application {ApplicationName} Starting", catalogApplication.ApplicationName);
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        });
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseCors("Catalog Cors");

    app.UseAuthorization();

    app.MapControllers();
    app.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Files/Images")),
        RequestPath = "/Files/Images"
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Files/Images")),
        RequestPath = "/Files/Images"
    });

    var eventBus = app.Services.GetService<IEventBus>();

    eventBus!.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
    eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

    //***** Custom Extension Methods - WEB APPLICATIONS *****\\\
    app.MigrateDbContext();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application {Application} Failes", catalogApplication.AppNamespace);
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }