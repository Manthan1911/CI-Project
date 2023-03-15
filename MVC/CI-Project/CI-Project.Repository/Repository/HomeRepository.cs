using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class HomeRepository : IHomeRepository
    {
        readonly CIProjectDbContext _db;
        public HomeRepository(CIProjectDbContext db)
        {
            _db = db;
        }
        public List<Country> getAllCountries()
        {
            return _db.Countries.ToList();
        }
        public List<City> getAllCities()
        {
            return _db.Cities.ToList();
        }

        public List<MissionTheme> getAllThemes()
        {
            return _db.MissionThemes.ToList();
        }

        public List<Skill> getAllSkills()
        {
            return _db.Skills.ToList();
        }

        public List<Mission> getAllMissions()
        {
            return _db.Missions
                .Include(m => m.GoalMissions)
                .Include(m => m.MissionApplications)
                .Include(m => m.MissionMedia)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.Theme)
                .Include(m => m.City)
                .Include(m => m.Country)
                .ToList();
        }

        public MissionMedium? getAllMissionMediaRows(long? id)
        {
            return _db.MissionMedia.Where(x => x.MissionId == id).FirstOrDefault();
        }

        public List<Mission> searchMissionAccToTitle(string missionToSearch)
        {
            
            return getAllMissions().Where(mission => mission.Title.ToLower().Contains(missionToSearch)).ToList();    
        }
    }
}
