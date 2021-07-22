using System.Threading.Tasks;

namespace ICEWeatherExercise.Core.Contracts
{
    public interface IWeatherForecastFileParser
    {
        Task<double> GetTemperature(double lon, double lat, string weatherForecastFilePath);
    }
}