using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Models;
using UsersApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UsersApp.Controllers
{
    public class DatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
