using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class ClinicViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Clinic name is required")]
        [Display(Name = "Clinic Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Working Hours")]
        public string WorkingHours { get; set; } = string.Empty;
    }
} 