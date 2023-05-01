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
		private readonly IBannerRepository _bannerRepository;


		public AuthenticationController(IUserRepository userRepository,IBannerRepository bannerRepository, IHttpContextAccessor httpContextAccessor, IUnitOfService unitOfService)
		{
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
			_unitOfService = unitOfService;
			_bannerRepository= bannerRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		// GET:Login
		public IActionResult Login()
		{
			LoginModel loginModel = new();

			loginModel.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();
			return View(loginModel);
		}

		[HttpPost]
		public IActionResult Login(LoginModel loginModelObj)
		{
			loginModelObj.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();

			var isUserAdmin = _userRepository.IsUserAdmin(loginModelObj.EmailId);

			if(isUserAdmin)
			{
				var adminUser = _userRepository.GetAllAdmin().FirstOrDefault(admin => admin.Email.Equals(loginModelObj.EmailId));
				if(loginModelObj.Password.Equals(adminUser?.Password))
				{
                    HttpContext.Session.SetString("IsAdmin", "true");
                    HttpContext.Session.SetString("adminEmail", loginModelObj.EmailId);
                    HttpContext.Session.SetString("adminId",adminUser.AdminId.ToString());
                    HttpContext.Session.SetString("adminFullName", adminUser.FirstName+" "+adminUser.LastName);

                    return RedirectToAction("Index", "Admin");
				}
				else
				{
                    return View(loginModelObj);
                }
			}

			var isEmailValid = _userRepository.validateEmail(loginModelObj.EmailId);
			var user = _userRepository.findUser(loginModelObj.EmailId);

			if (user.Status == false)
			{
				ViewBag.userStatus = "inactive";
				return View(loginModelObj);
			}

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
				if (user.Avatar!=null)
				{
                    HttpContext.Session.SetString("Avtar", user.Avatar?.ToString());
                }
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return View(loginModelObj);
			}
		}

		public IActionResult ForgotPassword()
		{
			ForgotPasswordModel forgotPasswordModel = new();

			forgotPasswordModel.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();
			return View(forgotPasswordModel);
		}

		[HttpPost]
		public IActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModelObj)
		{
			forgotPasswordModelObj.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();

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
					client.Authenticate("coder5255@gmail.com", "");
					client.Send(message);
					client.Disconnect(true);
				}

				return View(forgotPasswordModelObj);
			}
			else
			{
				return View(forgotPasswordModelObj);
			}
		}

		public IActionResult Registration()
		{
			RegisterationModel registerationModel = new();
			registerationModel.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();

			return View(registerationModel);
		}

		[HttpPost]
		public IActionResult Registration(RegisterationModel registerationModel)
		{
			registerationModel.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();
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
				return View(registerationModel);
			}
		}

		public IActionResult ResetPassword(string token)
		{
			ResetPasswordModel resetPassword = new ResetPasswordModel();
			resetPassword.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();

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
				return View(resetPassword);
			}

			TimeSpan remainingTime = (TimeSpan)(DateTime.Now - resetObj.CreatedAt);

			int hour = remainingTime.Hours;

			if (hour >= 4)
			{
				_userRepository.removeResetPasswordToken(resetObj);
				return RedirectToAction("Login");
			}

			resetPassword.token = token;
			return View(resetPassword);
		}

		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			resetPasswordModel.BannerImages = _bannerRepository.GetBannerModelList().OrderBy(banner => banner.SortOrder).ToList();

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
