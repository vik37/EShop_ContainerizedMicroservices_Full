
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
IConfiguration configuration = builder.Configuration;

builder.Services.Configure<APIUrlsOptionSettings>(configuration);

// Custom Extension Methods
services.HttpClientConfig(configuration);

services.AddSingleton<Retry>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "customerorder",
    pattern: "customerorder/ordersummary/{userId}",
    defaults: new { controller = "CustomerOrder", action = "OrderSummary" });
app.MapControllerRoute(
    name: "customerorder",
    pattern: "customerorder/{action}/{userId}/user/{orderId?}",
    defaults: new { controller = "CustomerOrder" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
