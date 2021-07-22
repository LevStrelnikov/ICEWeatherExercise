using System;
using System.Threading.Tasks;

namespace ICEWeatherExercise.Contracts
{
    public interface IWeatherForecastProvider
    {
        Task<WeatherForecast> GetForecast(DateTime date, double lon, double lat);
    }
}