using CI_Project.Entities.ViewModels;
using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using CI_Project.Services.Interface;

namespace CI_Platform_Web.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUnitOfService _unitOfService;


		public AuthenticationController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUnitOfService unitOfService)
		{
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
			_unitOfService = unitOfService;
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
			var user = _userRepository.findUser(loginModelObj.EmailId);

			var userId = user.UserId;
			var fullName = user.FirstName + " " + user.LastName;

			if (isEmailValid)
			{
				var decryptedPasswordOfFoundUser = _unitOfService.Password.Decode(user.Password);

				var isUserValid = decryptedPasswordOfFoundUser.Equals(loginModelObj.Password);

				if (!isUserValid)
				{
					ModelState.AddModelError("Password", "Password didn't match... Please try again");
				}
			}
			else
			{
				ModelState.AddModelError("EmailId", "Email not found");
			}

			if (ModelState.IsValid)
			{

				HttpContext.Session.SetString("UserEmail", loginModelObj.EmailId);
				HttpContext.Session.SetString("UserId", userId.ToString());
				HttpContext.Session.SetString("FullName", fullName);
				HttpContext.Session.SetString("Avtar", user.Avatar?.ToString());
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

			if (!isEmailValid)
			{
				ModelState.AddModelError("EmailId", "Email not found ... Maybe you are not an existing user!");
			}

			if (ModelState.IsValid)
			{
				var userObj = _userRepository.findUser(forgotPasswordModelObj.EmailId);

				string uuid = Guid.NewGuid().ToString();
				PasswordReset resetPasswordObj = new PasswordReset()
				{
					Email = userObj.Email,
					Token = uuid,
					CreatedAt = DateTime.Now,
				};

				_userRepository.addResetPasswordToken(resetPasswordObj);
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("CI-Platform", "gohelumang12@gmail.com"));
				message.To.Add(new MailboxAddress("User", forgotPasswordModelObj.EmailId));
				message.Subject = "CI-Platform RESET PASSWORD";
				message.Body = new TextPart("html")
				{
					Text = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Authentication/ResetPassword?token=" + uuid + " \"  style=\"font-weight:500;color:blue;\" > Click here to Reset Your Password </a>"
				};
				using (var client = new SmtpClient())
				{
					client.Connect("smtp.gmail.com", 587, false);
					client.Authenticate("coder5255@gmail.com", "icxemzbfyycvvzpd");
					client.Send(message);
					client.Disconnect(true);
				}

				return View();
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
					user.FirstName = registerationModel.FirstName;
					user.LastName = registerationModel.LastName;
					user.PhoneNumber = long.Parse(registerationModel.PhoneNo);
					user.Email = registerationModel.EmailId;
					user.Password = _unitOfService.Password.Encode(registerationModel.Password);
					user.CreatedAt = DateTime.Now;
					var IsUserAdded = _userRepository.addUser(user);
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

		public IActionResult ResetPassword(string token)
		{
			PasswordReset resetObj;


			try
			{
				resetObj = _userRepository.findUserByToken(token);

				if (resetObj.Token == null)
				{
					throw new Exception("token might have expired!");
				}
			}
			catch (Exception)
			{
				return View("ForgotPassword");
			}

			TimeSpan remainingTime = (TimeSpan)(DateTime.Now - resetObj.CreatedAt);

			int hour = remainingTime.Hours;

			if (hour >= 4)
			{
				_userRepository.removeResetPasswordToken(resetObj);
				return RedirectToAction("Login");
			}

			ResetPasswordModel resetPassword = new ResetPasswordModel();
			resetPassword.token = token;
			return View(resetPassword);
		}

		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
		{

			if (resetPasswordModel.NewPassword.Equals(resetPasswordModel.ConfirmPassword))
			{
				string token = resetPasswordModel.token;
				var resetObj = _userRepository.findUserByToken(token);
				var user = _userRepository.findUser(resetObj.Email);

				if (resetPasswordModel.NewPassword.Equals(_unitOfService.Password.Decode(user.Password)))
				{
					ModelState.AddModelError("NewPassword", "You cannot set your OldPassword as NewPassword");
				}
				else
				{
					try
					{
						user.Password = _unitOfService.Password.Encode(resetPasswordModel.NewPassword);
						user.Password = _unitOfService.Password.Encode(resetPasswordModel.NewPassword);
						var IsPasswordUpdated = _userRepository.updatePassword(user);
						if (!IsPasswordUpdated)
						{
							throw new Exception("Some problem occured while saving changes...Please try again!");
						}
						_userRepository.removeResetPasswordToken(resetObj);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex + " : " + ex.Message);
					}
					return RedirectToAction("Login");
				}
			}
			else
			{
				ModelState.AddModelError("ConfirmPassword", "ConfirmPassword and NewPassword don't match");
			}
			return View(resetPasswordModel);
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Authentication");
		}

		[HttpGet]
		public IActionResult PageNotFound()
		{

			return View();
		}
	}


}
