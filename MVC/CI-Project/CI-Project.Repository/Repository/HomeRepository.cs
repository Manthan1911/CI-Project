using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

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
    }
}
