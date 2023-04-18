using EShop.Basket.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace EShop.Basket.API.Controllers
{
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
            RedisExample re = new();
            string recordId = "WeatherForecast_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
;
            re.Forecast = await _cache.GetRecordAsync<IEnumerable<WeatherForecast>>(recordId);

            if(re.Forecast is null)
            {
                re.Forecast = GetData;

                re.LoadLocation = $"Loaded from API at {DateTime.Now}";
                await _cache.SetRecordAsync<IEnumerable<WeatherForecast>>(recordId, re.Forecast);
            }
            else
            {
                re.LoadLocation = $"Loaded from Cache {DateTime.Now}";
                re.IsCacheData = true;
            }
            return re;
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
}