using CI_Platform_Web.Utilities;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	[Authenticate]
	public class FooterPages : Controller
	{
		private readonly IUserRepository _userRepository;

		public FooterPages(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IActionResult GetContactUsPartial()
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			User user = _userRepository.findUser(userEmailId);

			ContactUsModel contactUs = new ContactUsModel()
			{
				UserId = user.UserId,
				UserName = user.FirstName + " " + user.LastName,
				EmailAddress = user.Email,
			};
			return PartialView("_ContactUs", contactUs);
		}

		public IActionResult SaveContactUsData(ContactUsModel contact)
		{
			try
			{
				ContactU contactUsOj = new ContactU()
				{
					UserId = contact.UserId,
					Subject = contact.Subject,
					Message = contact.Message,
					Status = false,
				};

				_userRepository.saveContactUsData(contactUsOj);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return NoContent();
			}
			return Ok(200);
		}
	}
}
