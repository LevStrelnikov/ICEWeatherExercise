using System;

namespace ICEWeatherExercise.Contracts.Storages
{
    public interface ILocalFileStorage
    {
        string GetFilePath(DateTime date, int hoursOffset);
        bool FileExists(string path);
    }
}