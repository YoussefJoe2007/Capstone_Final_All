using System.Collections.Generic;
using UsersApp.Models;

namespace UsersApp.ViewModels
{
    public class PatientDashboardViewModel
    {
        public Users User { get; set; } = null!;
        public List<Appointment> Appointments { get; set; } = new();
        public List<MedicalForm> MedicalForms { get; set; } = new();
        public int TotalForms { get; set; }
        public int TotalDoctors { get; set; }
        public DateTime? LastFormDate { get; set; }
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
} 