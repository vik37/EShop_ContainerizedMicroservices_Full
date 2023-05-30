
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

// Add services to the container.
var services = builder.Services;

var application = Application.GetApplication();

IConfiguration configuration = builder.Configuration;

//***** Custom Extension Methods - Application Configurations *****\\\

services.SwaggerConfigurations()
        .DatabaseConfiguration(application.DockerMSQLConnectionString(configuration))
        .CorsConfiguration()
        .ApiVersioning()
        .ConfigurationEventBus(configuration, retryConnection: application.RabbitMQRetry(configuration))
        .RegisterEventBusRabbitMQ(subscriptionClientName: configuration["SubscriptionClientName"], application.RabbitMQRetry(configuration));
        

services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

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

    Log.Information("Application {ApplicationName} Starting", application.ApplicationName);
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

    //***** Custom Extension Methods - WEB APPLICATIONS *****\\\
    app.MigrateDbContext();

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

public partial class Program { }