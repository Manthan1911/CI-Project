using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
	public class UsersSkill : IUsersSkill
	{
		private readonly CIProjectDbContext _db;
		public UsersSkill(CIProjectDbContext db)
		{
			_db = db;
		}
		public List<UserSkill> GetUserSkills()
		{
			return _db.UserSkills.ToList();
		}
	}
}
