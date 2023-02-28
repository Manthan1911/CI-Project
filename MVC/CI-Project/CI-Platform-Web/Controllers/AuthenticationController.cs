using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using CI_Project.Entities.DataModels;

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
                var isEmailValid = _userRepository.validateEmail(loginModelObj.EmailId);
                var isUserValid = _userRepository.validateUser(loginModelObj.EmailId,loginModelObj.Password);
                if (isEmailValid)
                {
                    if (!isUserValid) 
                    { 
                        ModelState.AddModelError("Password", "Password didn't match... Please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailId", "Email not found");
                }

                if (ModelState.IsValid) {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModelObj)
        {
            var isEmailValid = _userRepository.validateEmail(forgotPasswordModelObj.EmailId);

            if(!isEmailValid)
            {
                ModelState.AddModelError("EmailId", "Email not found ... Maybe you are not an existing user!");
            }

            if (ModelState.IsValid)
            { 
                var userObj = _userRepository.findUser(forgotPasswordModelObj.EmailId);
                return RedirectToAction("ResetPassword","Authentication",new { id = userObj.UserId });
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

        [HttpPost]
        public IActionResult Registration(RegisterationModel registerationModel)
        {
            var IsUserAlreadyRegistered = _userRepository.validateEmail(registerationModel.EmailId);

            if (IsUserAlreadyRegistered)
            {
                ModelState.AddModelError("EmailId", "Email aleady Exists!");
            }
            else
            {
                if (registerationModel.Password.Equals(registerationModel.ConfirmPassword))
                {
                    User user = new User();
                    user.FirstName= registerationModel.FirstName;
                    user.LastName= registerationModel.LastName;
                    user.PhoneNumber = long.Parse(registerationModel.PhoneNo);
                    user.Email = registerationModel.EmailId;
                    user.Password= registerationModel.Password;
                    user.CreatedAt= DateTime.Now;
                    var IsUserAdded =_userRepository.addUser(user);
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Password and ConfirmPassword don't match!");
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public IActionResult ResetPassword(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel,int? id)
        {   

            if (resetPasswordModel.NewPassword.Equals(resetPasswordModel.ConfirmPassword))
            {
                var user = _userRepository.findUser(id);

                if (resetPasswordModel.NewPassword.Equals(user.Password))
                {
                    ModelState.AddModelError("NewPassword", "You cannot set your OldPassword as NewPassword");
                }
                else
                {
                    try
                    {
                        user.Password= resetPasswordModel.NewPassword;
                        var IsPasswordUpdated = _userRepository.updatePassword(user);
                    }
                    catch
                    {
                        throw new Exception("Some problem occured while saving changes...Please try again!");
                    }
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ModelState.AddModelError("ConfirmPassword","ConfirmPassword and NewPassword don't match");
            }
            return View();
        }
    }
}
