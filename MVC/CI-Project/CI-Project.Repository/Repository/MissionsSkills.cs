using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace CI_Project.Repository.Repository
{
	public class MissionsSkills : IMissionsSkill
	{
		private readonly CIProjectDbContext _db;

		public MissionsSkills(CIProjectDbContext db)
		{
			_db= db;
		}

        public void AddMissionSkill(MissionSkill missionSkill)
        {
			_db.MissionSkills.Add(missionSkill);
			_db.SaveChanges();
        }

		public void DeleteListOfMissionSkills(List<MissionSkill> missionSkills)
		{
			_db.MissionSkills.RemoveRange(missionSkills);
			_db.SaveChanges();	
		}

		public List<MissionSkill> GetAllMissionSkills()
		{
			return _db.MissionSkills.ToList();
		}
	}
}
