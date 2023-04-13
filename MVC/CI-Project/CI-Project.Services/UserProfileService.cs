using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
	public class UserProfileService : IUserProfileService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserProfileService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public UserProfileModel GetUserProfileById(long? id)
		{
			return ConvertUserProfileToViewModel(_unitOfWork.UserProfile.GetUser(id));
		}

		public UserProfileModel ConvertUserProfileToViewModel(User user)
		{
			return new UserProfileModel()
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
				Cities = _unitOfWork.UserProfile.getCityList(),
				Countries = _unitOfWork.UserProfile.getCountryList(),
				Skills = _unitOfWork.UserProfile.getSkillList(),
				UserSkills = _unitOfWork.UserProfile.getListOfUserSkill(user.UserId),
				Avtar = user.Avatar,
			};
		}

		public User GetUserById(long? userId)
		{
			User user =  _unitOfWork.UserProfile.GetUser(userId);
			if (user == null)
			{
				throw new Exception("User not found");
			}
			return user;
		}

		public void UpdateUser(User user)
		{
			var isUserUpdated = _unitOfWork.UserProfile.updateUser(user);

			if (isUserUpdated < 0)
			{
				throw new Exception("Some Error occured while Updating User");
			}
		}

		public List<UserSkill> GetListOfUserSkill(long userId)
		{
			return _unitOfWork.UserProfile.getListOfUserSkill(userId);
		}

		public void DeleteAllSkillsOfUser(long userId)
		{
			List<UserSkill> userSkills = _unitOfWork.UserProfile.getListOfUserSkill(userId);
			var areUserSkillsDeleted = _unitOfWork.UserProfile.deleteAllSkillsOfUser(userSkills);

			if (areUserSkillsDeleted < 0)
			{
				throw new Exception("Some Error occured while deleting User's skills");
			}
		}

		public void SaveUserSkill(int[] skills,long userId)
		{
			foreach (var currSkill in skills)
			{
				UserSkill userSkill = new UserSkill()
				{
					UserId = userId,
					SkillId = currSkill,
					CreatedAt = DateTime.Now,
				};

				var isSkillAdded = _unitOfWork.UserProfile.saveUserSkill(userSkill);
				if (isSkillAdded < 0)
				{
					throw new Exception("Some Error occured while saving User's skill");
				}
			}
		}


		public void UpdateUserPassword(User user,string encodedPassword)
		{
			user.Password = encodedPassword;
			var isUserPasswordUpdated = _unitOfWork.UserProfile.updateUser(user);

			if (isUserPasswordUpdated < 0)
			{
				throw new Exception("(UserProfileService) : Some Error occured while saving User's Password");
			}
		}
	}
}
