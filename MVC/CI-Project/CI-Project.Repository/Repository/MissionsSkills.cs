using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace CI_Project.Repository.Repository
{
	public class MissionsSkills : IMissionsSkills
	{
		private readonly CIProjectDbContext _db;

		public MissionsSkills(CIProjectDbContext db)
		{
			_db= db;
		}

        public void AddMissionSkill(MissionSkills missionSkill)
        {
			_db.MissionSkills.Add(missionSkill);
			_db.SaveChanges();
        }

		public void DeleteListOfMissionSkills(List<MissionSkills> missionSkills)
		{
			_db.MissionSkills.RemoveRange(missionSkills);
			_db.SaveChanges();	
		}

		public List<MissionSkills> GetAllMissionSkills()
		{
			return _db.MissionSkills.ToList();
		}
	}
}
