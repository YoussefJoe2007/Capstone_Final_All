using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Capstone_Final.Controllers
{
    [AllowAnonymous]
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 