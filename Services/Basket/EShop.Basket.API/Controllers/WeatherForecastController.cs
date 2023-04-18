using EShop.Basket.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace EShop.Basket.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDistributedCache _cache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<RedisExample> Get()
    {
        RedisExample RE = new();
        string recordId = "WeatherForecast_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
        ;
        RE.Forecast = await _cache.GetRecordAsync<IEnumerable<WeatherForecast>>(recordId);

        if (RE.Forecast is null)
        {
            RE.Forecast = GetData;

            RE.LoadLocation = $"Loaded from API at {DateTime.Now}";
            await _cache.SetRecordAsync<IEnumerable<WeatherForecast>>(recordId, RE.Forecast);
        }
        else
        {
            RE.LoadLocation = $"Loaded from Cache {DateTime.Now}";
            RE.IsCacheData = true;
        }
        return RE;
    }

    private static IEnumerable<WeatherForecast> GetData =>
        Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

}
public class RedisExample
{
    public string LoadLocation { get; set; }
    public bool IsCacheData { get; set; } = false;
    public IEnumerable<WeatherForecast> Forecast { get; set; }
}