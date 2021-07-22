using System;
using System.Threading.Tasks;
using ICEWeatherExercise.Contracts;
using ICEWeatherExercise.Contracts.Storages;
using ICEWeatherExercise.Core.Services;
using Microsoft.Extensions.Logging;

namespace ICEWeatherExercise.Core
{
    public class WeatherForecastProvider : IWeatherForecastProvider
    {
        private readonly ILocalFileStorage _localFileStorage;
        private readonly IRemoteFileStorage _remoteFileStorage;
        private readonly IWeatherForecastFileParser _weatherForecastFileParser;
        private readonly ILogger<WeatherForecastProvider> _logger;

        public WeatherForecastProvider(ILocalFileStorage localFileStorage, IRemoteFileStorage remoteFileStorage,
            IWeatherForecastFileParser weatherForecastFileParser, ILogger<WeatherForecastProvider> logger)
        {
            _localFileStorage = localFileStorage;
            _remoteFileStorage = remoteFileStorage;
            _weatherForecastFileParser = weatherForecastFileParser;
            _logger = logger;
        }

        public async Task<WeatherForecast> GetForecast(DateTime dateTime, double lon, double lat)
        {
            _logger.LogInformation(
                $"Getting forecast for following parameters: {nameof(dateTime)}: {dateTime}, {nameof(lat)}: {lat}, {nameof(lon)}: {lon}");

            (DateTime date, int hoursOffset) = ResolveForecastFileParameters(dateTime);

            var localFilePath = _localFileStorage.GetFilePath(date, hoursOffset);
            _logger.LogInformation($"Searching for cached file for: {nameof(date)}: {date}, {nameof(hoursOffset)}: {hoursOffset} in {localFilePath}");

            if (!_localFileStorage.FileExists(localFilePath))
            {
                _logger.LogInformation($"Cached file for: {nameof(date)}: {date}, {nameof(hoursOffset)}: {hoursOffset} wasn't found");
                _logger.LogInformation($"Downloading file for: {nameof(date)}: {date}, {nameof(hoursOffset)}: {hoursOffset} from remote storage");

                await _remoteFileStorage.DownloadFile(date, hoursOffset, localFilePath);
            }

            _logger.LogInformation($"Extracting temperature for: {nameof(lon)}: {lon}, {nameof(lat)}: {lat} from file");

            var temperature = await _weatherForecastFileParser.GetTemperature(lon, lat, localFilePath);

            return createWeatherForecast(temperature, lon, lat);
        }

        public (DateTime date, int hoursOffset) ResolveForecastFileParameters(DateTime dateTime)
        {
            var today = DateTime.UtcNow.Date;
            if (dateTime.Date < today)
            {
                return (dateTime.Date, dateTime.Hour);
            }

            return (today, (int)(dateTime - today).TotalHours);
        }

        private WeatherForecast createWeatherForecast(double temperature, double lon, double lat)
        {
            return new WeatherForecast
            {
                lon = lon,
                lat = lat,
                Temperature = new Temperature
                {
                    Kelvin = temperature,
                    Celsius = TemperatureConverter.ToCelsius(temperature),
                    Fahrenheit = TemperatureConverter.ToFahrenheit(temperature)
                }
            };
        }
    }
}