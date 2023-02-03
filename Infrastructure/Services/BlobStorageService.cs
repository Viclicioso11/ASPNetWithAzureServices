using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobClient;
        private readonly IConfiguration _config;
        private readonly ILogger<BlobStorageService> _logger;
        public BlobStorageService(BlobServiceClient blobClient, IConfiguration config, ILogger<BlobStorageService> logger)
        {
            _blobClient = blobClient;
            _config = config;
            _logger = logger;
        }

        public async Task<Stream?> GetFileByName(string fileName)
        {
            try
            {
                var container = _blobClient.GetBlobContainerClient(_config["BlobStorageConfig:ContainerName"]);

                var blobClient = container.GetBlobClient(fileName);

                if (blobClient.Exists())
                {
                    var content = await blobClient.DownloadStreamingAsync();

                    return content.Value.Content;
                }

                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error has ocurred trying to get the file with messsage => {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UploadFile(string fileName, Stream file)
        {
            try
            {
                var container = _blobClient.GetBlobContainerClient(_config["BlobStorageConfig:ContainerName"]);

                var blobClient = container.GetBlobClient(fileName);

                file.Position = 0;

                if (!blobClient.Exists())
                {
                    await container.UploadBlobAsync(fileName, file);

                    return true;
                }
                else
                {
                    await blobClient.UploadAsync(file, overwrite: true);
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error has ocurred trying to upload the file with messsage => {ex.Message}");
                return false;
            }
        }
    }
}
