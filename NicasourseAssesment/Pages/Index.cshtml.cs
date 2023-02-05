using Application.Common.Interfaces;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NicasourseAssesment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFileDBService _dbService;

        public IndexModel(ILogger<IndexModel> logger, IFileDBService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        public bool IsLoading { get; set; } = true;

        public List<FileMetadata>? Files { get; set; } = new List<FileMetadata>();

        public async Task OnGet()
        {
            var files = await _dbService.GetFilesByUserId("victorabud11@gmail.com");

            IsLoading = false;

            Files = files;
        }

        public async Task OnPost()
        {

        }



    }
}