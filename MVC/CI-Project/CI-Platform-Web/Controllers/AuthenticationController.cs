using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CI_Platform_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET:Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModelObj)
        {
            if(ModelState.IsValid){

                var isEmailValid = _userRepository.validateEmail(loginModelObj.EmailId);
                var isUserValid = _userRepository.validateUser(loginModelObj.EmailId,loginModelObj.Password);
                if (isEmailValid)
                {
                    if (isUserValid)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password didn't match... Please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailId", "Email not found");
                }

                return View();
            }
            else
            {
                return View(loginModelObj);
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModelObj)
        {
            if (ModelState.IsValid) {
                return View("ResetPassword");
            }
            else
            {
                return View();
            }
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
