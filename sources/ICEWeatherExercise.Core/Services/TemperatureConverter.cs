namespace ICEWeatherExercise.Core.Services
{
    public static class TemperatureConverter
    {
        public static double ToCelsius(double tempInKelvin)
        {
            return tempInKelvin - 273.15;
        }

        public static double ToFahrenheit(double tempInKelvin)
        {
            return (tempInKelvin - 273.15) * 9 / 5 + 32;
        }
    }
}