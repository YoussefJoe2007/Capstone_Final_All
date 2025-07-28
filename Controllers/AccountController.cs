using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using UsersApp.Models;
using UsersApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace UsersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "PatientDashboard");
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    Role = model.Role // Set the role from the form
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    // Add user to role
                    await userManager.AddToRoleAsync(users, model.Role);
                    
                    await signInManager.SignInAsync(users, isPersistent: false);
                    
                    // Redirect based on role
                    if (model.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "PatientDashboard");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                // المستخدم موجود بالفعل، جيب أدواره ووجهه دائمًا لصفحة PatientDashboard
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                {
                    return RedirectToAction(nameof(Login));
                }
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }
                // أضف المستخدم لدور Patient إذا لم يكن مضافًا
                if (!await userManager.IsInRoleAsync(user, "Patient"))
                {
                    await userManager.AddToRoleAsync(user, "Patient");
                    // إعادة تسجيل الدخول لتحديث الـ Claims
                    await signInManager.SignInAsync(user, isPersistent: false);
                }
                // حتى لو كان Admin، وجهه دائمًا للـ PatientDashboard بعد تسجيل جوجل
                return RedirectToAction("Dashboard", "PatientDashboard");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var fullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email ?? "Unknown User";

                if (email != null)
                {
                    // إذا الاسم غير موجود أو غير صالح، اطلب من المستخدم إدخاله
                    if (string.IsNullOrWhiteSpace(fullName) || fullName == email || fullName == "Unknown User")
                    {
                        // مرر الإيميل وreturnUrl لصفحة إدخال الاسم والدور
                        return RedirectToAction("ExternalRole", new { email = email, returnUrl = returnUrl });
                    }

                    var user = new Users
                    {
                        UserName = email,
                        Email = email,
                        FullName = fullName
                    };

                    var createResult = await userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        // إضافة تسجيل الدخول الخارجي
                        await userManager.AddLoginAsync(user, info);
                        // تعيين دور Patient تلقائياً للمستخدمين الجدد من جوجل
                        await userManager.AddToRoleAsync(user, "Patient");
                        // تسجيل الدخول
                        await signInManager.SignInAsync(user, isPersistent: false);
                        // توجيه لصفحة Patient Dashboard
                        return RedirectToAction("Dashboard", "PatientDashboard");
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpGet]
        public IActionResult ExternalRole(string email, string returnUrl)
        {
            return View(new ExternalRoleViewModel { Email = email, ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> ExternalRole(ExternalRoleViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (string.IsNullOrEmpty(model.Role))
            {
                ModelState.AddModelError("Role", "Please select a role.");
                return View(model);
            }

            // تحديث الاسم إذا تم إدخاله
            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                user.FullName = model.FullName;
                await userManager.UpdateAsync(user);
            }

            await userManager.AddToRoleAsync(user, model.Role);
            await signInManager.SignInAsync(user, isPersistent: false);

            if (model.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");
            else
                return RedirectToAction("Dashboard", "PatientDashboard");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
