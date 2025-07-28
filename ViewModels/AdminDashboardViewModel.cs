using System.Collections.Generic;
using UsersApp.Models;

namespace UsersApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalClinics { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalUsers { get; set; }
        public List<Users> Users { get; set; } = new List<Users>();
        public List<Appointment> RecentAppointments { get; set; } = new();
    }
} 