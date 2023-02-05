using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace NicasourseAssesment.Models.ViewModels
{
    public class AddFileViewModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The file is required")]
        public IFormFile? FormFile { get; set; }
    }
}
