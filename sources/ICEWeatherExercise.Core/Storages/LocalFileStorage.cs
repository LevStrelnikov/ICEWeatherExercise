using System;
using System.IO;
using System.Reflection;
using Amazon.Runtime.Internal;
using ICEWeatherExercise.Contracts.Storages;
using Microsoft.Extensions.Configuration;

namespace ICEWeatherExercise.Core.Storages
{
    public class LocalFileStorage : ILocalFileStorage
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseDirectory;

        public LocalFileStorage(IConfiguration  configuration)
        {
            _configuration = configuration;
            _baseDirectory = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, _configuration.GetSection("LocalStorage")["baseDirectoryPath"]);
        }

        public string GetFilePath(DateTime date, int hoursOffset)
        {
            return Path.Combine(_baseDirectory, $"forecast_{date:yyyy-MM-dd}_{hoursOffset}");
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);

        }
    }
}