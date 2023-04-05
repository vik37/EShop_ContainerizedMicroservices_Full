using EShop.Catalog.API;
using Newtonsoft.Json.Serialization;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.
var services = builder.Services;

IConfiguration configuration = builder.Configuration;

//***** Custom Extension Methods - Application Configurations *****\\\
services.SwaggerConfigurations()
        .DatabaseConfiguration(config: configuration)
        .CorsConfiguration()
        .ApiVersioning();

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

    Log.Information("Application {ApplicationName} Starting", Application.GetApplication().ApplicationName);
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

    //***** Custom Extension Methods - WEB APPLICATIONS *****\\\
    app.MigrateDbContext();

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
