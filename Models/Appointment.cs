using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public class Appointment
    {
        public int Id { get; set; }

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

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign keys
        public string PatientId { get; set; } = string.Empty;
        public int DoctorId { get; set; }

        // Navigation properties
        public virtual Users Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public DateTime UpdatedAt { get;  set; }
    }
} 