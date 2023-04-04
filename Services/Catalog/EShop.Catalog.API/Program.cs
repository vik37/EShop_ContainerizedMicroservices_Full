using EShop.Catalog.API;
using EShop.Catalog.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

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

services.AddDbContext<CatalogDbContext>(opt =>
{
    if (configuration != null)
    {
        string dbConnectionString = configuration["ConnectionString"] ?? throw new ArgumentNullException();
        opt.UseSqlServer(dbConnectionString,
        sqlServerOptionsAction: sqlOption =>
        {
            sqlOption.MigrationsAssembly(
                    Assembly.GetExecutingAssembly().GetName().Name
                );
            sqlOption.EnableRetryOnFailure(maxRetryCount: 5,
                                            maxRetryDelay: TimeSpan.FromSeconds(30),
                                            errorNumbersToAdd: null);
        });
    }
});

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShopOnContainers - Catalog HTTP API",
        Version = "v1",
        Description = "The Catalog Microservice HTTP API. This is a DataDriven/CRUD microservice sample"
    });
});

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

    app.UseAuthorization();

    app.MapControllers();

    //***** Custom Extension Methods *****\\\
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
