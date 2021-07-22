using System;

namespace ICEWeatherExercise.Core.Contracts
{
    public interface ILocalFileStorage
    {
        string GetFilePath(DateTime date, int hoursOffset);
        bool FileExists(string path);
    }
}