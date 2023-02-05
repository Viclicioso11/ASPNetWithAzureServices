using Application.Common.Interfaces;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NicasourseAssesment.Pages.Files
{
    public class ListModel : PageModel
    {
        private readonly IFileDBService _dbService;
        private readonly IBlobStorageService _blobStorageService;

        public ListModel(IFileDBService dbService, IBlobStorageService blobStorageService)
        {
            _dbService = dbService;
            _blobStorageService = blobStorageService;
        }

        public bool IsLoading { get; set; } = true;

        public List<FileMetadata>? Files { get; set; } = new List<FileMetadata>();

        public async Task OnGet()
        {
            var files = await _dbService.GetFilesByUserId("victorabud11@gmail.com");

            IsLoading = false;

            Files = files;
        }

        public async Task<ActionResult> OnGetDownloadFile(string fileName, string filePath)
        {
            var file = await _blobStorageService.GetFileByName(filePath);

            var fileMemoryStream = new MemoryStream();

            await file!.CopyToAsync(fileMemoryStream);

            return File(fileMemoryStream.ToArray(), "application/octet-stream", fileName);
        }
    }
}
