using UsersApp.Models;
using Microsoft.AspNetCore.Identity;

namespace UsersApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Patient"))
            {
                await roleManager.CreateAsync(new IdentityRole("Patient"));
            }

            // Create test admin user
            var adminEmail = "admin@test.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    Role = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create test patient user
            var patientEmail = "patient@test.com";
            var patientUser = await userManager.FindByEmailAsync(patientEmail);
            if (patientUser == null)
            {
                patientUser = new Users
                {
                    UserName = patientEmail,
                    Email = patientEmail,
                    FullName = "Test Patient",
                    Role = "Patient",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(patientUser, "Patient123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(patientUser, "Patient");
                }
            }

            // Create test clinics if they don't exist
            if (!context.Clinics.Any())
            {
                var clinics = new List<Clinic>
                {
                    new Clinic
                    {
                        Name = "Cardiology Clinic",
                        Address = "King Fahd Street, Riyadh",
                        PhoneNumber = "011-456-7890",
                        Description = "Specialized clinic for heart and cardiovascular diseases",
                        WorkingHours = "Sunday - Thursday: 8 AM - 5 PM"
                    },
                    new Clinic
                    {
                        Name = "Pediatrics Clinic",
                        Address = "Tahlia Street, Jeddah",
                        PhoneNumber = "012-234-5678",
                        Description = "Specialized clinic for children's health and care",
                        WorkingHours = "Sunday - Thursday: 9 AM - 6 PM"
                    },
                    new Clinic
                    {
                        Name = "Ophthalmology Clinic",
                        Address = "Olaya Street, Riyadh",
                        PhoneNumber = "011-345-6789",
                        Description = "Specialized clinic for eye care and surgery",
                        WorkingHours = "Sunday - Thursday: 8 AM - 4 PM"
                    }
                };

                context.Clinics.AddRange(clinics);
                await context.SaveChangesAsync();
            }

            // Create test doctors if they don't exist
            if (!context.Doctors.Any())
            {
                var clinics = context.Clinics.ToList();
                var doctors = new List<Doctor>
                {
                    new Doctor
                    {
                        FirstName = "Ahmed",
                        LastName = "Mohammed",
                        PhoneNumber = "050-123-4567",
                        Speciality = "Cardiology",
                        City = "Riyadh",
                        Experience = 15,
                        Description = "Specialized doctor in heart and cardiovascular diseases with 15 years experience",
                        ConsultationFee = 300.00m,
                        ClinicId = clinics.First().Id,
                        IsActive = true
                    },
                    new Doctor
                    {
                        FirstName = "Fatima",
                        LastName = "Ali",
                        PhoneNumber = "050-234-5678",
                        Speciality = "Pediatrics",
                        City = "Jeddah",
                        Experience = 12,
                        Description = "Specialized doctor in children's health and care with 12 years experience",
                        ConsultationFee = 250.00m,
                        ClinicId = clinics.Skip(1).First().Id,
                        IsActive = true
                    },
                    new Doctor
                    {
                        FirstName = "Khalid",
                        LastName = "Abdullah",
                        PhoneNumber = "050-345-6789",
                        Speciality = "Ophthalmology",
                        City = "Riyadh",
                        Experience = 18,
                        Description = "Specialized doctor in eye care and surgery with 18 years experience",
                        ConsultationFee = 400.00m,
                        ClinicId = clinics.Skip(2).First().Id,
                        IsActive = true
                    }
                };

                context.Doctors.AddRange(doctors);
                await context.SaveChangesAsync();
            }
        }
    }
} 