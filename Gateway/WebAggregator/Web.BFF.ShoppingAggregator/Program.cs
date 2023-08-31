
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Configuration;

builder.Services.Configure<APIUrlsOptionSettings>(configuration);

IServiceCollection services = builder.Services;

services.AddControllers()
    .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

services.AddCors(options =>
{
    options.AddPolicy("Web BFF Cors", options =>
    {
        options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShopOnContainers - WEB - Backend For Frontend",
        Version = "v1",
        Description = "The Web BFF Aggregation"
    });
});

services.AddHttpClient<IOrderService,OrderService>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { PooledConnectionLifetime = new TimeSpan(5) });
services.AddHttpClient<IBasketService,BasketService>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { PooledConnectionLifetime = new TimeSpan(5) });

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

app.UseCors("Web BFF Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
