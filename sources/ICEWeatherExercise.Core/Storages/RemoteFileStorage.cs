using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using ICEWeatherExercise.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ICEWeatherExercise.Core.Storages
{
    public class RemoteFileStorage : IRemoteFileStorage
    {
        private readonly ILogger<RemoteFileStorage> _logger;
        private readonly string _bucketName;
        private readonly AmazonS3Client _client;

        public RemoteFileStorage(IConfiguration configuration, ILogger<RemoteFileStorage> logger)
        {
            _logger = logger;
            _bucketName = configuration.GetSection("RemoteStorage")["awsBucketName"];
            var bucketRegion =
                Amazon.RegionEndpoint.GetBySystemName(configuration.GetSection("RemoteStorage")["BucketRegion"]);
            _client = new AmazonS3Client(new AnonymousAWSCredentials(), bucketRegion);
        }

        public async Task DownloadFile(DateTime date, int hoursOffset, string targetPath)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = getFileUrl(date, hoursOffset)
            };

            _logger.LogInformation($"Getting '{request.Key}' file from AWS '{request.BucketName}' bucket");
            using GetObjectResponse response = await _client.GetObjectAsync(request);

            _logger.LogInformation(
                $"Starting to write '{request.Key}' file from AWS '{request.BucketName}' bucket to {targetPath}");
            await response.WriteResponseStreamToFileAsync(targetPath, false, CancellationToken.None);
            _logger.LogInformation(
                $"Completed to write '{request.Key}' file from AWS '{request.BucketName}' bucket to {targetPath}");
        }

        private string getFileUrl(DateTime date, int hoursOffset)
        {
            return $"gfs.{date:yyyyMMdd}/00/atmos/gfs.t00z.pgrb2.0p25.f{hoursOffset:D3}";
        }
    }
}