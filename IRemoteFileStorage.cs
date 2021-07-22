using System;
using System.Threading.Tasks;

namespace ICEWeatherExercise.Core.Contracts
{
    public interface IRemoteFileStorage
    {
        Task DownloadFile(DateTime date, int hoursOffset, string targetPath);
    }
}