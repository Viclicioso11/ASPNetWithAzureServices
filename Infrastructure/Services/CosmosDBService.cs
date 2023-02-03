using Application.Common.Interfaces;
using Domain.Entities.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class CosmosDBService : ICosmosDBService
    {
        private readonly IConfiguration _config;
        private readonly CosmosClient _client;

        public CosmosDBService(IConfiguration config)
        {
            _config = config;
            _client = new CosmosClient(
                connectionString: _config["CosmosDB:ConnectionString"]
            );
        }
        private Container container
        {
            get => _client
                .GetDatabase(_config["CosmosDB:DBName"])
                .GetContainer(_config["CosmosDB:ContainerName"]);
        }

        public async Task<List<FileMetadata>> GetFilesByUserId(string userId)
        {
            var sql = "" +
                "SELECT " +
                "f.Id, " +
                "f.UserId, " +
                "f.FileName, " +
                "f.Path, " +
                "f.CanBeRetrieved " +
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
    }
}
