
namespace Domain.Entities.Entities
{
    public class FileMetadata
    {
        public string? Id { get; set; }

        public string? UserId { get; set; }

        public string? FileName { get; set; }

        public string? Path { get; set; }

        public bool CanBeRetrieved { get; set; }    

    }
}
