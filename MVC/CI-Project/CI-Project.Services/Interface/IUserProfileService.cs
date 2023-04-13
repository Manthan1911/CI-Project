using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Services.Interface
{
	public interface IUserProfileService
	{
		public UserProfileModel GetUserProfileById(long? id);

		public User GetUserById(long? userId);

		public void UpdateUser(User user);

		public List<UserSkill> GetListOfUserSkill(long userId);

		public void DeleteAllSkillsOfUser(long userId);

		public void SaveUserSkill(int[] skills, long userId);

		public void UpdateUserPassword(User user, string encodedPassword);

	}
}
