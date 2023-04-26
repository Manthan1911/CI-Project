using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{



    public class MissionRepository : IMissionRepository
    {
        private readonly CIProjectDbContext _db;

        public MissionRepository(CIProjectDbContext db)
        {
            _db = db;
        }

        public void AddMission(Mission mission)
        {
            _db.Missions.Add(mission);
            _db.SaveChanges();
        }

        public void UpdateMission(Mission mission)
        {
            _db.Missions.Update(mission);
            _db.SaveChanges();
        }

        public List<Mission> GetAllMissionsWithInclude()
        {
            return _db.Missions
                .Include(m => m.MissionMedia)
                .Include(m => m.GoalMissions)
                .Include(m => m.MissionApplications)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionSkills)
                .Include(m => m.Theme)
                .Include(m => m.City)
                .Include(m => m.Country)
                .ToList();
        }

    }
}
