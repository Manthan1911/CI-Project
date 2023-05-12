using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IUserProfileRepository: IRepository<User>
	{
		public User GetUser(long? userId);

		public List<City> getCityList();
		
		public List<Country> getCountryList();

		public List<Skill> getSkillList();

		public List<UserSkill> getListOfUserSkill(long userId);

		public int updateUser(User user);

		public int deleteAllSkillsOfUser(List<UserSkill> userSkills);

		public int saveUserSkill(UserSkill userSkill);


	}
}
