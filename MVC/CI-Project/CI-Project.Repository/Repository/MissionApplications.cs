using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
	public class MissionApplications : IMissionApplication
	{
		private readonly CIProjectDbContext _db;

		public MissionApplications(CIProjectDbContext db)
		{
			_db = db;
		}

		public void AddMissionApplication(MissionApplication missionApplication)
		{
			_db.Add(missionApplication);
			_db.SaveChanges();
		}

		public List<MissionApplication> GetAllMissionApplicationsWithInclude()
		{
			return _db.MissionApplications
				.Include(missionApplication => missionApplication.Mission)
				.Include(missionApplication => missionApplication.User)
				.ToList();
		}

		public MissionApplication GetMissionApplicationById(long missionApplicationId)
		{
			return _db.MissionApplications.FirstOrDefault(missionApplication => missionApplication.MissionApplicationId == missionApplicationId);
		}

		public void UpdaateMissionApplication(MissionApplication missionApplication)
		{
			_db.MissionApplications.Update(missionApplication);
			_db.SaveChanges();
		}
		public void DeleteMissionApplication(MissionApplication missionApplication)
		{
			_db.MissionApplications.Remove(missionApplication);
			_db.SaveChanges();
		}
	}
}