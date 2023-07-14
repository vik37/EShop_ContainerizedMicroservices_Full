using EShop.Orders.API;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR( cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    
});

builder.Services.SwaggerConfigurations()
                .DatabaseConfiguration(configuration["OrderingDbConnection"]??"");

builder.Services.AddTransient<IOrderQuery, OrderQuery>(o =>
{
    return new OrderQuery(configuration["OrderingDbConnection"]??"");
});

var app = builder.Build();

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

app.MigrateDbContext();

app.Run();
