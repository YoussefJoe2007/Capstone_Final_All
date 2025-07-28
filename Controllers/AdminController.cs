using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;
using UsersApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public AdminController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalPatients = (await _userManager.GetUsersInRoleAsync("Patient")).Count;
            var totalDoctors = await _context.Doctors.CountAsync();
            var totalClinics = await _context.Clinics.CountAsync();
            var totalAppointments = await _context.Appointments.CountAsync();
            var recentAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .ToListAsync();
            var users = await _context.Users.ToListAsync();
            var totalUsers = users.Count;

            var viewModel = new AdminDashboardViewModel
            {
                TotalPatients = totalPatients,
                TotalDoctors = totalDoctors,
                TotalClinics = totalClinics,
                TotalAppointments = totalAppointments,
                RecentAppointments = recentAppointments,
                Users = users,
                TotalUsers = totalUsers
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null && appointment.Status == AppointmentStatus.Pending)
            {
                appointment.Status = AppointmentStatus.Confirmed;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        // Clinic Management
        public async Task<IActionResult> Clinics()
        {
            var clinics = await _context.Clinics
                .Include(c => c.Doctors)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(clinics);
        }

        public IActionResult AddClinic()
        {
            return View(new ClinicViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(ClinicViewModel model)
        {
            if (ModelState.IsValid)
            {
                var clinic = new Clinic
                {
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Description = model.Description,
                    WorkingHours = model.WorkingHours
                };

                _context.Clinics.Add(clinic);
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم إضافة العيادة بنجاح";
                return RedirectToAction(nameof(Clinics));
            }

            return View(model);
        }

        public async Task<IActionResult> EditClinic(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }

            var viewModel = new ClinicViewModel
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Description = clinic.Description,
                WorkingHours = clinic.WorkingHours
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClinic(ClinicViewModel model)
        {
            if (ModelState.IsValid)
            {
                var clinic = await _context.Clinics.FindAsync(model.Id);
                if (clinic == null)
                {
                    return NotFound();
                }

                clinic.Name = model.Name;
                clinic.Address = model.Address;
                clinic.PhoneNumber = model.PhoneNumber;
                clinic.Description = model.Description;
                clinic.WorkingHours = model.WorkingHours;

                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تحديث العيادة بنجاح";
                return RedirectToAction(nameof(Clinics));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف العيادة بنجاح";
            }
            return RedirectToAction(nameof(Clinics));
        }

        // Doctor Management
        public async Task<IActionResult> Doctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Clinic)
                .OrderBy(d => d.FirstName)
                .ToListAsync();

            return View(doctors);
        }

        public async Task<IActionResult> AddDoctor()
        {
            ViewBag.Clinics = await _context.Clinics.OrderBy(c => c.Name).ToListAsync();
            return View(new DoctorRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Speciality = model.Speciality,
                    City = model.City,
                    Experience = model.Experience,
                    Description = model.Description,
                    ConsultationFee = model.ConsultationFee,
                    ClinicId = model.ClinicId
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم إضافة الطبيب بنجاح";
                return RedirectToAction(nameof(Doctors));
            }

            ViewBag.Clinics = await _context.Clinics.OrderBy(c => c.Name).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> EditDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.Clinics = await _context.Clinics.OrderBy(c => c.Name).ToListAsync();
            
            var viewModel = new DoctorRegisterViewModel
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                PhoneNumber = doctor.PhoneNumber,
                Speciality = doctor.Speciality,
                City = doctor.City,
                Experience = doctor.Experience,
                Description = doctor.Description,
                ConsultationFee = doctor.ConsultationFee,
                ClinicId = doctor.ClinicId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(DoctorRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = await _context.Doctors.FindAsync(model.Id);
                if (doctor == null)
                {
                    return NotFound();
                }

                doctor.FirstName = model.FirstName;
                doctor.LastName = model.LastName;
                doctor.PhoneNumber = model.PhoneNumber;
                doctor.Speciality = model.Speciality;
                doctor.City = model.City;
                doctor.Experience = model.Experience;
                doctor.Description = model.Description;
                doctor.ConsultationFee = model.ConsultationFee;
                doctor.ClinicId = model.ClinicId;

                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تحديث الطبيب بنجاح";
                return RedirectToAction(nameof(Doctors));
            }

            ViewBag.Clinics = await _context.Clinics.OrderBy(c => c.Name).ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الطبيب بنجاح";
            }
            return RedirectToAction(nameof(Doctors));
        }

        // Appointment Management
        public async Task<IActionResult> Appointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Clinic)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, int status)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = (AppointmentStatus)status;
            appointment.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
} 