using Application.Common.Interfaces;
using Domain.Entities.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class FileDBService : IFileDBService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileDBService> _logger;
        private readonly CosmosClient _client;

        public FileDBService(IConfiguration config, ILogger<FileDBService> logger)
        {
            _config = config;
            _client = new CosmosClient(
                connectionString: _config["CosmosDB:ConnectionString"]
            );
            _logger = logger;
        }
        private Container container
        {
            get => _client
                .GetDatabase(_config["CosmosDB:DBName"])
                .GetContainer(_config["CosmosDB:ContainerName"]);
        }

        public async Task<bool> CreateFile(FileMetadata file)
        {
            try
            {
                await container.CreateItemAsync(file);
                return true;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, $"An error has occured trying to create the file with message {ex.Message}");
                return false;
            }
        }

        public async Task<FileMetadata?> GetFileById(string fileId)
        {
            var sql = "" +
                "SELECT " +
                "f.id, " +
                "f.UserId, " +
                "f.FileName, " +
                "f.FilePath, " +
                "f.Description " +
                "FROM FileMetadata f " +
                "WHERE f.id = @fileId";

            var query = new QueryDefinition(
                query: sql
            ).WithParameter("@fileId", fileId);

            using FeedIterator<FileMetadata> feed = container.GetItemQueryIterator<FileMetadata>(
                queryDefinition: query
            );

            List<FileMetadata> results = new();

            while (feed.HasMoreResults)
            {
                FeedResponse<FileMetadata> response = await feed.ReadNextAsync();
                foreach (FileMetadata item in response)
                {
                    results.Add(item);
                }
            }

            return results?.FirstOrDefault();
        }

        public async Task<List<FileMetadata>> GetFilesByUserId(string userId)
        {
            var sql = "" +
                "SELECT " +
                "f.id, " +
                "f.UserId, " +
                "f.FileName, " +
                "f.FilePath, " +
                "f.Description " +
                "FROM FileMetadata f " +
                "WHERE f.UserId = @userId";

            var query = new QueryDefinition(
                query: sql
            ).WithParameter("@userId", userId);

            using FeedIterator<FileMetadata> feed = container.GetItemQueryIterator<FileMetadata>(
                queryDefinition: query
            );

            List<FileMetadata> results = new();

            while (feed.HasMoreResults)
            {
                FeedResponse<FileMetadata> response = await feed.ReadNextAsync();
                foreach (FileMetadata item in response)
                {
                    results.Add(item);
                }
            }

            return results;
        }

        public async Task<bool> UpdateFile(FileMetadata file)
        {
            try
            {
                await container.UpsertItemAsync(file);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error has occured trying to update the file with message {ex.Message}");
                return false;
            }
        }
    }
}
