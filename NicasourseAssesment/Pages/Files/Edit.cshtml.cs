using Application.Common.Interfaces;
using Application.ViewModels;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NicasourseAssesment.Pages.Files
{
    public class EditModel : PageModel
    {
        private readonly IFileDBService _dbService;

        public EditModel(IFileDBService dbService)
        {
            _dbService = dbService;
        }

        public bool IsLoading { get; set; } = true;

        [BindProperty]
        public EditFileViewModel EditFileViewModel { get; set; }

        public async Task OnGet(string id)
        {
            var file = await _dbService.GetFileById(id);

            if (file != null)
            {
                EditFileViewModel = new EditFileViewModel
                {
                    Id = file.id,
                    Description = file.Description
                };
            }

            IsLoading = false;
        }

        public async Task OnPost()
        {
            IsLoading = true;

            var file = await _dbService.GetFileById(EditFileViewModel.Id!);

            file!.Description = EditFileViewModel.Description;

            var wasUpdated = await _dbService.UpdateFile(file);

            if (wasUpdated)
            {
                ViewData["Message"] = "File updated successfully";
                ViewData["HasError"] = false;
            }
            else
            {
                ViewData["Message"] = "An error ocurred updating the file, please try again";
                ViewData["HasError"] = true;
            }

            IsLoading = false;
        }
    }
}
