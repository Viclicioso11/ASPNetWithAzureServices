using Domain.Entities.Entities;

namespace Application.Common.Interfaces
{
    public interface IFileDBService
    {
        /// <summary>
        /// Get the file data by id
        /// </summary>
        /// <param name="fileId">The id of the file</param>
        /// <returns>The file associated to the id</returns>
        Task<FileMetadata?> GetFileById(string fileId);

        /// <summary>
        /// Get the list of files associated to a user
        /// </summary>
        /// <param name="userId">The id of the user owner of the files</param>
        /// <returns>The list of files associated to the user id</returns>
        Task<List<FileMetadata>> GetFilesByUserId(string userId);

        /// <summary>
        /// Updates the file metadata in database
        /// </summary>
        /// <param name="file">The file object</param>
        /// <returns>True if was successfully updated, otherwise false</returns>
        Task<bool> UpdateFile(FileMetadata file);

        /// <summary>
        /// Saves a file metadata in database
        /// </summary>
        /// <param name="file">The file object</param>
        /// <returns>True if was successfully saved, otherwise false</returns>
        Task<bool> CreateFile(FileMetadata file);
    }
}
