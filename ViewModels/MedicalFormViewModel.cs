using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class MedicalFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Height")]
        public string Height { get; set; } = string.Empty;

        [Display(Name = "Weight")]
        public string Weight { get; set; } = string.Empty;

        [Display(Name = "Blood Type")]
        public string BloodType { get; set; } = string.Empty;

        [Display(Name = "Chronic Diseases")]
        public string ChronicDiseases { get; set; } = string.Empty;

        [Display(Name = "Allergies")]
        public string Allergies { get; set; } = string.Empty;

        [Display(Name = "Current Medications")]
        public string CurrentMedications { get; set; } = string.Empty;

        [Display(Name = "Family History")]
        public string FamilyHistory { get; set; } = string.Empty;

        [Display(Name = "Current Symptoms")]
        public string CurrentSymptoms { get; set; } = string.Empty;

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpeciality { get; set; } = string.Empty;
    }
} 