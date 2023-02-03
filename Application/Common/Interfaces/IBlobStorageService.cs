namespace Application.Common.Interfaces
{
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads a file to the solution container blob storage
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="file">File content</param>
        /// <returns>True if succeded, false otherwise</returns>
        Task<bool> UploadFile(string fileName, Stream file);

        /// <summary>
        /// Get the content file by its name
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>If not found null, otherwise the file content</returns>
        Task<Stream?> GetFileByName(string fileName);
    }
}
