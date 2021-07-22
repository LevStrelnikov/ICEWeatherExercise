namespace ICEWeatherExercise.Core.Contracts
{
    public class WeatherForecast
    {
        public double lon { get; set; }
        public double lat { get; set; }

        public Temperature Temperature { get; set; }
    }

    public class Temperature
    {
        public double Kelvin { get; set; }
        public double Celsius { get; set; }
        public double Fahrenheit { get; set; }
    }
}