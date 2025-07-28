using System.ComponentModel.DataAnnotations;
namespace UsersApp.ViewModels
{
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpeciality { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
    }
} 