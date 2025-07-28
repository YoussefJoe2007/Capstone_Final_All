using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class DoctorRegisterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Speciality is required")]
        [Display(Name = "Speciality")]
        public string Speciality { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Experience (Years)")]
        public int Experience { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Consultation Fee")]
        [Required]
        [Range(0, 10000)]
        public decimal ConsultationFee { get; set; }

        [Required(ErrorMessage = "Clinic is required")]
        [Display(Name = "Clinic")]
        public int? ClinicId { get; set; }
    }
}
