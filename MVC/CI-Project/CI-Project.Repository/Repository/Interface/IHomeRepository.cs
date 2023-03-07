using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IHomeRepository
    {
        public List<Country> getAllCountries();
        public List<City> getAllCities();   

        public List<MissionTheme> getAllThemes();
        public List<Skill> getAllSkills();
    }
}
