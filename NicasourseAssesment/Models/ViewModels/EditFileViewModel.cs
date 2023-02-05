using System.ComponentModel.DataAnnotations;

namespace NicasourseAssesment.Models.ViewModels
{
    public class EditFileViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
    }
}
