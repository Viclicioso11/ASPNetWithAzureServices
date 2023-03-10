using Application.Common.Interfaces;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NicasourseAssesment.Models.ViewModels;

namespace NicasourseAssesment.Pages.Files
{
    public class AddModel : PageModel
    {
        private readonly IFileDBService _dbService;
        private readonly IBlobStorageService _blobStorageService;

        public AddModel(IFileDBService dbService, IBlobStorageService blobStorageService)
        {
            _dbService = dbService;
            _blobStorageService = blobStorageService;
        }

        public bool IsLoading { get; set; } = false;

        [BindProperty]
        public AddFileViewModel AddFileRequest { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            IsLoading = true;
            var id = Guid.NewGuid().ToString();
            var fileName = AddFileRequest.FormFile!.FileName;
            var userId = User.Claims.First(cl => cl.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value; ;

            using (var memoryStream = new MemoryStream())
            {
                await AddFileRequest.FormFile!.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var filedSaved = await _blobStorageService.UploadFile($"{userId}/{id}/{fileName}", memoryStream);

                    if (!filedSaved)
                    {
                        ViewData["Message"] = "An error ocurred saving the file, please try again";
                        ViewData["HasError"] = true;
                        return;
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            var fileMetadata = new FileMetadata
            {
                id = id,
                UserId = userId,
                Description = AddFileRequest.Description,
                FileName = $"{fileName}",
                FilePath = $"{userId}/{id}/{fileName}"
            };

            var wasSaved = await _dbService.CreateFile(fileMetadata);

            if (wasSaved)
            {
                ViewData["Message"] = "File saved successfully";
                ViewData["HasError"] = false;
            }
            else
            {
                ViewData["Message"] = "An error ocurred saving the file, please try again";
                ViewData["HasError"] = true;
            }

            IsLoading = false;
        }
    }
}
