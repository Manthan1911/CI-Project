using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	public class UserProfileController : Controller
	{
		public readonly IUserRepository _userRepository;
		public readonly IUserProfileRepository _userProfileRepository;

		public UserProfileController(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
		{
			_userRepository = userRepository;
			_userProfileRepository = userProfileRepository;
		}

		public IActionResult Index(long? userId)
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			User user = _userProfileRepository.GetUser(userId);
			UserProfileModel userProfileModel;
			if (user != null)
			{

				userProfileModel = new UserProfileModel()
				{
					UserId = user.UserId,
					EmailId = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					PhoneNo = user.PhoneNumber,
					Title = user.Title,
					Department = user.Department,
					EmployeeId = user.EmployeeId,
					MyProfile = user.ProfileText,
					WhyIVolunteer = user.WhyIVolunteer,
					LinkedIn = user.LinkedInUrl,
					CountryId = user.CountryId,
					CityId = user.CityId,
					Availability = user.Availability,
				};
				userProfileModel.Cities = _userProfileRepository.getCityList();
				userProfileModel.Countries = _userProfileRepository.getCountryList();
				userProfileModel.Skills = _userProfileRepository.getSkillList();
				userProfileModel.UserSkills = _userProfileRepository.getListOfUserSkill(userProfileModel.UserId);
			}
			else
			{
				userProfileModel = new();
			}

			return View(userProfileModel);
		}

		[HttpPost]
		public IActionResult Index(UserProfileModel userProfileModel, int[] skills,IFormFile profileImage)
		{
			if (userProfileModel != null)
			{
				try
				{
					User user = _userProfileRepository.GetUser(userProfileModel.UserId);

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

					var isUserUpdated = _userProfileRepository.updateUser(user);

					if (isUserUpdated < 0)
					{
						throw new Exception("Some Error occured while Updating User");
					}

					if (skills.Length > 0)
					{
						List<UserSkill> userSkills = _userProfileRepository.getListOfUserSkill(userProfileModel.UserId);
						var areUserSkillsDeleted = _userProfileRepository.deleteAllSkillsOfUser(userSkills);
						if (areUserSkillsDeleted < 0)
						{
							throw new Exception("Some Error occured while deleting User's skills");
						}

						foreach (var currSkill in  skills)
						{
							UserSkill userSkill = new UserSkill()
							{
								UserId = userProfileModel.UserId,
								SkillId = currSkill,
								CreatedAt= DateTime.Now,
							};

							var isSkillAdded = _userProfileRepository.saveUserSkill(userSkill);
							if (isSkillAdded < 0)
							{
								throw new Exception("Some Error occured while saving User's skill");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message.ToString());
				}
			}
			return RedirectToAction("Index","Home");
		}
	}
}
