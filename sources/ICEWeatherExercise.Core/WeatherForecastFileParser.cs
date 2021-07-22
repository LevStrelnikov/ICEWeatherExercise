using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ICEWeatherExercise.Contracts;
using Microsoft.Extensions.Logging;

namespace ICEWeatherExercise.Core
{
    public class WeatherForecastFileParser : IWeatherForecastFileParser
    {
        private readonly ILogger<WeatherForecastFileParser> _logger;

        public WeatherForecastFileParser(ILogger<WeatherForecastFileParser> logger)
        {
            _logger = logger;
        }

        public async Task<double> GetTemperature(double lon, double lat, string weatherForecastFilePath)
        {
            var processArguments =
                string.Join(" ", weatherForecastFilePath, "-match", "\":(TMP:2 m above ground):\"",
                    $"-lon {lon} {lat}");

            var wGrib2Path = getWGrib2Path();

            _logger.LogInformation($"Calling for '{wGrib2Path}' with arguments {processArguments} ");
            using var process = Process.Start(new ProcessStartInfo(wGrib2Path, processArguments)
                {RedirectStandardOutput = true, RedirectStandardError = true});

            process?.WaitForExit();

            _logger.LogInformation($"'wgrib2.exe' completed with {process.ExitCode} exit code");

            if (process.ExitCode != 0)
            {
                var error =
                    $"'wgrib2.exe' failed with exit code {process.ExitCode}. Error response: {await process.StandardError.ReadToEndAsync()}";
                _logger.LogError(error);

                throw new Exception(error);
            }

            var response = await process.StandardOutput.ReadToEndAsync();
            _logger.LogInformation($"'wgrib2.exe' completed successfully with response '{response}'");


            return getTemperatureFromResponse(response);
        }

        private string getWGrib2Path()
        {
            var wGrib2ExePath = Path.Combine(Directory.GetParent(Assembly.GetCallingAssembly().Location).FullName,
                "Resources", "wgrib2", "wgrib2.exe");
            Debug.Assert(File.Exists(wGrib2ExePath));

            return wGrib2ExePath;
        }

        private double getTemperatureFromResponse(string response)
        {
            try
            {
                return double.Parse(response.Split("val=").Last());
            }
            catch(Exception ex)
            {
                var error = $"Failed to extract temperature from 'wgrib2.exe' response '{response}'";
                _logger.LogError(error);

                throw new Exception(error, ex);
            }
        }
    }
}