using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;
using UsersApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientDashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public PatientDashboardController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new UsersApp.ViewModels.PatientDashboardViewModel
            {
                User = currentUser,
                Appointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Clinic)
                    .Where(a => a.PatientId == currentUser.Id)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                MedicalForms = await _context.MedicalForms
                    .Include(m => m.Doctor)
                    .Where(m => m.PatientId == currentUser.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // View Doctors
        public async Task<IActionResult> Doctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Clinic)
                .Where(d => d.IsActive)
                .OrderBy(d => d.FirstName)
                .ToListAsync();

            return View(doctors);
        }

        public async Task<IActionResult> DoctorDetails(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // View Clinics
        public async Task<IActionResult> Clinics()
        {
            var clinics = await _context.Clinics
                .Include(c => c.Doctors.Where(d => d.IsActive))
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(clinics);
        }

        public async Task<IActionResult> ClinicDetails(int id)
        {
            var clinic = await _context.Clinics
                .Include(c => c.Doctors.Where(d => d.IsActive))
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // Book Appointment
        public async Task<IActionResult> BookAppointment(int doctorId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.Id == doctorId && d.IsActive);

            if (doctor == null)
            {
                return NotFound();
            }

            var viewModel = new AppointmentViewModel
            {
                DoctorId = doctorId,
                DoctorName = $"{doctor.FirstName} {doctor.LastName}",
                DoctorSpeciality = doctor.Speciality,
                ClinicName = doctor.Clinic?.Name ?? "Unknown Clinic"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var appointment = new Appointment
                {
                    PatientId = currentUser.Id,
                    DoctorId = model.DoctorId,
                    Date = model.Date,
                    Time = model.Time,
                    Notes = model.Notes,
                    Status = AppointmentStatus.Pending
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Appointment booked successfully";
                return RedirectToAction(nameof(MyAppointments));
            }

            // Reload doctor info if validation fails
            var doctor = await _context.Doctors
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.Id == model.DoctorId);

            if (doctor != null)
            {
                model.DoctorName = $"{doctor.FirstName} {doctor.LastName}";
                model.DoctorSpeciality = doctor.Speciality;
                model.ClinicName = doctor.Clinic?.Name ?? "Unknown Clinic";
            }

            return View(model);
        }

        // My Appointments
        public async Task<IActionResult> MyAppointments()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Clinic)
                .Where(a => a.PatientId == currentUser.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == currentUser.Id);

            if (appointment == null)
            {
                return Json(new { success = false, message = "Appointment not found" });
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Appointment cancelled successfully" });
        }

        // Medical Forms
        public async Task<IActionResult> MedicalForms()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var forms = await _context.MedicalForms
                .Include(m => m.Doctor)
                .Where(m => m.PatientId == currentUser.Id)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(forms);
        }

        public async Task<IActionResult> CreateMedicalForm(int doctorId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.Id == doctorId && d.IsActive);

            if (doctor == null)
            {
                return NotFound();
            }

            var viewModel = new MedicalFormViewModel
            {
                DoctorId = doctorId,
                DoctorName = $"{doctor.FirstName} {doctor.LastName}",
                DoctorSpeciality = doctor.Speciality
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMedicalForm(MedicalFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Verify that the doctor exists and is active
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Id == model.DoctorId && d.IsActive);
                
                if (doctor == null)
                {
                    // Log the issue for debugging
                    var allDoctors = await _context.Doctors.ToListAsync();
                    var doctorIds = string.Join(", ", allDoctors.Select(d => d.Id));
                    
                    ModelState.AddModelError("DoctorId", $"Selected doctor (ID: {model.DoctorId}) not found or inactive. Available doctor IDs: {doctorIds}");
                    
                    // Repopulate the model with doctor info for the view
                    var originalDoctor = await _context.Doctors
                        .FirstOrDefaultAsync(d => d.Id == model.DoctorId);
                    if (originalDoctor != null)
                    {
                        model.DoctorName = $"{originalDoctor.FirstName} {originalDoctor.LastName}";
                        model.DoctorSpeciality = originalDoctor.Speciality;
                    }
                    
                    return View(model);
                }

                var medicalForm = new MedicalForm
                {
                    PatientId = currentUser.Id,
                    DoctorId = model.DoctorId,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Height = model.Height,
                    Weight = model.Weight,
                    BloodType = model.BloodType,
                    ChronicDiseases = model.ChronicDiseases,
                    Allergies = model.Allergies,
                    CurrentMedications = model.CurrentMedications,
                    FamilyHistory = model.FamilyHistory,
                    CurrentSymptoms = model.CurrentSymptoms,
                    AdditionalNotes = model.AdditionalNotes
                };

                _context.MedicalForms.Add(medicalForm);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Medical form created successfully";
                return RedirectToAction(nameof(MedicalForms));
            }

            return View(model);
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var medicalForm = await _context.MedicalForms
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id && m.PatientId == currentUser.Id);

            if (medicalForm == null)
            {
                return NotFound();
            }

            // Check if this is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Return partial view for modal
                return PartialView("_MedicalFormDetailsPartial", medicalForm);
            }

            // For now, return a view instead of PDF to avoid iText issues
            return View("MedicalFormDetails", medicalForm);
        }
    }
} 