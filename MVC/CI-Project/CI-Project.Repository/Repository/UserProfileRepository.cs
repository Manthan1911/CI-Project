using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Project.Repository.Repository
{
	public class UserProfileRepository : IUserProfileRepository
	{
		private readonly CIProjectDbContext _db;

		public UserProfileRepository(CIProjectDbContext db)
		{
			_db = db;
		}

		public User GetUser(long? userId)
		{
			return _db.Users.FirstOrDefault(u => u.UserId == userId);
		}

		public List<City> getCityList()
		{
			return _db.Cities.ToList();
		}

		public List<Country> getCountryList()
		{
			return _db.Countries.ToList();
		}

		public List<Skill> getSkillList()
		{
			return _db.Skills.ToList();
		}

		public List<UserSkill> getListOfUserSkill(long userId)
		{
			return _db.UserSkills.Where(us => us.UserId == userId).ToList();	
		}

		public int updateUser(User user)
		{
			_db.Users.Update(user);
			return _db.SaveChanges();
		}

		public int deleteAllSkillsOfUser(List<UserSkill> userSkills)
		{
			_db.UserSkills.RemoveRange(userSkills);
			return _db.SaveChanges();
		}

		public int saveUserSkill(UserSkill userSkill)
		{
			_db.UserSkills.Add(userSkill);
			return _db.SaveChanges();
		}

	}
}
