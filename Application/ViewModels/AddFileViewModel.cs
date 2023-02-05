using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class AddFileViewModel
    {
        [Required]
        public string? Description { get; set; }

        [Required]
        public IFormFile? FormFile { get; set; }
    }
}
