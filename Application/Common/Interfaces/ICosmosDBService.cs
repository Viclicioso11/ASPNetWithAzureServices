using Domain.Entities.Entities;

namespace Application.Common.Interfaces
{
    public interface ICosmosDBService
    {
        Task<List<FileMetadata>> GetFilesByUserId(string userId);
    }
}
