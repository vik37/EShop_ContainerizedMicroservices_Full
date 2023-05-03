using EShop.Web.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

// Custom Extension Methods
services.HttpClientConfig(configuration);

services.AddTransient<ICatalogService, CatalogService>();
services.AddTransient<IBasketService, BasketService>();
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
