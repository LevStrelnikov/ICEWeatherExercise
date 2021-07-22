using System;
using System.Threading.Tasks;
using ICEWeatherExercise.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ICEWeatherExercise.ApiControllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMemoryCache _weatherCache;
        private readonly IWeatherForecastProvider _forecastProvider;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IMemoryCache weatherCache, IWeatherForecastProvider forecastProvider,
            ILogger<WeatherForecastController> logger)
        {
            _weatherCache = weatherCache;
            _forecastProvider = forecastProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("/forecast/{date}/{lat}/{lon}")]
        public async Task<IActionResult> GetWeatherForecast(DateTime date, double lat, double lon)
        {
            _logger.LogInformation(
                $"{nameof(GetWeatherForecast)} API was called with following parameters: {nameof(date)}: {date}, {nameof(lat)}: {lat}, {nameof(lon)}: {lon}");

            var cacheKey = new {Today = DateTime.Now.Date, Date = date, Lat = lat, Lon = lon};

            if (_weatherCache.TryGetValue(cacheKey, out WeatherForecast forecast))
            {
                _logger.LogInformation(
                    $"Value for '{string.Join("", cacheKey.Today, cacheKey.Date, cacheKey.Lat, cacheKey.Lon)}' was found in cache. Returning cached value");
            }
            else
            {
                _logger.LogInformation(
                    $"Getting forecast for following parameters: {nameof(date)}: {date}, {nameof(lat)}: {lat}, {nameof(lon)}: {lon}");
                forecast = await _forecastProvider.GetForecast(date, lon, lat);

                _weatherCache.Set(cacheKey, forecast, DateTimeOffset.Now + TimeSpan.FromDays(1));
            }

            return Ok(forecast);
        }
    }
}