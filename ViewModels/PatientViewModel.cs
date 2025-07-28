using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class PatientViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Range(0, 150, ErrorMessage = "Age must be between 0 and 150")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Range(0.0, 500.0, ErrorMessage = "Weight must be between 0 and 500")]
        [Display(Name = "Weight")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;
    }
}
