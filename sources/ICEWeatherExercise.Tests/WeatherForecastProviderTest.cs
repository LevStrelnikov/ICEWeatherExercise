using System;
using ICEWeatherExercise.Contracts;
using ICEWeatherExercise.Contracts.Storages;
using ICEWeatherExercise.Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ICEWeatherExercise.Tests
{
    public class WeatherForecastProviderTest
    {
        [Fact]
        public void ResolveForecastFileParametersTest()
        {
            WeatherForecastProvider forecastProvider = new WeatherForecastProvider(Mock.Of<ILocalFileStorage>(),
                Mock.Of<IRemoteFileStorage>(), Mock.Of<IWeatherForecastFileParser>(),
                Mock.Of<ILogger<WeatherForecastProvider>>());

            var datetime = DateTime.UtcNow.Date - TimeSpan.FromDays(1) + TimeSpan.FromHours(5);
            var actual = forecastProvider.ResolveForecastFileParameters(datetime);
            var expected = (DateTime.UtcNow.Date - TimeSpan.FromDays(1), 5);

            Assert.Equal(expected, actual);


            datetime = DateTime.UtcNow.Date + TimeSpan.FromHours(5);
            actual = forecastProvider.ResolveForecastFileParameters(datetime);
            expected = (DateTime.UtcNow.Date, 5);

            Assert.Equal(expected, actual);

            datetime = DateTime.UtcNow.Date + TimeSpan.FromDays(1) + TimeSpan.FromHours(5);
            actual = forecastProvider.ResolveForecastFileParameters(datetime);
            expected = (DateTime.UtcNow.Date, 29); // 24 + 5

            Assert.Equal(expected, actual);
        }
    }
}