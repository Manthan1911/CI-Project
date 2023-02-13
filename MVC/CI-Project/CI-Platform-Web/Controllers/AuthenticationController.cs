using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
