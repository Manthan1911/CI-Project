using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CI_Platform_Web.Controllers
{
	public class UserProfileController : Controller
	{
		private readonly IUnitOfService _unitOfService;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public UserProfileController(IUnitOfService unitOfService, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfService = unitOfService;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index(long? userId)
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			UserProfileModel userProfileModel = new();
			if (userId != null && userId > 0)
			{
				userProfileModel = _unitOfService.UserProfile.GetUserProfileById(userId);
				return View(userProfileModel);
			}
			return RedirectToAction("PageNotFound", "Authentication");
		}

		[HttpPost]
		public IActionResult Index(UserProfileModel userProfileModel, int[] skills, IFormFile profileImage)
		{
			if (userProfileModel != null)
			{
				try
				{
					User user = _unitOfService.UserProfile.GetUserById(userProfileModel.UserId);

					user.UserId = userProfileModel.UserId;
					user.FirstName = userProfileModel.FirstName?.Trim();
					user.LastName = userProfileModel.LastName?.Trim();
					user.PhoneNumber = userProfileModel.PhoneNo;
					user.Title = userProfileModel.Title?.Trim();
					user.Department = userProfileModel.Department?.Trim();
					user.EmployeeId = userProfileModel.EmployeeId?.Trim();
					user.ProfileText = userProfileModel.MyProfile?.Trim();
					user.WhyIVolunteer = userProfileModel.WhyIVolunteer?.Trim();
					user.LinkedInUrl = userProfileModel.LinkedIn?.Trim();
					user.CountryId = userProfileModel.CountryId;
					user.CityId = userProfileModel.CityId;
					user.Availability = userProfileModel.Availability;
					user.UpdatedAt = DateTime.Now;


					if (skills.Length > 0)
					{
						_unitOfService.UserProfile.DeleteAllSkillsOfUser(userProfileModel.UserId);
						_unitOfService.UserProfile.SaveUserSkill(skills, userProfileModel.UserId);
					}

					if (profileImage!=null)
					{
						if (profileImage.Length > 0 && profileImage.Length! > 1)
						{
							string wwwRootPath = _webHostEnvironment.WebRootPath;

							string fileName = Guid.NewGuid().ToString();
							var uploads = Path.Combine(wwwRootPath, @"images\profile_images");
							var extension = Path.GetExtension(profileImage?.FileName);

							using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
							{
								profileImage?.CopyTo(fileStrems);
							}

							if (userProfileModel.Avtar != null)
							{

								System.IO.File.Delete(Path.Combine(wwwRootPath, userProfileModel.Avtar.TrimStart('\\')));

							}

							user.Avatar = @"\images\profile_images\" + fileName + extension;

						}
					}
					_unitOfService.UserProfile.UpdateUser(user);
					HttpContext.Session.SetString("Avtar", user.Avatar?.ToString());

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message.ToString());
				}
			}
			return RedirectToAction("Index", "Home", new { profileSuccess = "true" });
		}

		[HttpGet]
		public IActionResult GetChangePasswordModal()
		{
			ChangePasswordModel changePasswordModel = new();
			return PartialView("_ChangePassword", changePasswordModel);
		}


		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordModel changePasswordModel)
		{
			if (!ModelState.IsValid)
			{
				return	RedirectToAction("Index","UserProfile",new { userId = changePasswordModel.UserId });
			}
			try
			{
				User user = _unitOfService.UserProfile.GetUserById(changePasswordModel.UserId);

				var isOldPasswordValid = _unitOfService.Password.Decode(user.Password).Equals(changePasswordModel.OldPassword) ? true : false;
				if (!isOldPasswordValid)
				{
					return NoContent();
				}

				string encodedPassword;
				if (changePasswordModel.NewPassword != null)
				{
					encodedPassword = _unitOfService.Password.Encode(changePasswordModel.NewPassword);
					_unitOfService.UserProfile.UpdateUserPassword(user, encodedPassword);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return Ok(200);
		}


		
	}
}
