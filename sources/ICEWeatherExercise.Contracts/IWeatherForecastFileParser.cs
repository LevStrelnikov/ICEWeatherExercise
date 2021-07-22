using System.Threading.Tasks;

namespace ICEWeatherExercise.Contracts
{
    public interface IWeatherForecastFileParser
    {
        Task<double> GetTemperature(double lon, double lat, string weatherForecastFilePath);
    }
}