using System;
using System.Threading.Tasks;

namespace ICEWeatherExercise.Contracts.Storages
{
    public interface IRemoteFileStorage
    {
        Task DownloadFile(DateTime date, int hoursOffset, string targetPath);
    }
}