using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IHomeRepository
    {
        public List<Country> getAllCountries();
        public List<City> getAllCities();   

        public List<MissionTheme> getAllThemes();
        public List<Skill> getAllSkills();

        public List<Mission> getAllMissions();

        public MissionMedium? getAllMissionMediaRows(long? id);

        public List<Mission> searchMissionAccToTitle(string missionToSearch,List<Mission> missions);

        public void addOrRemoveFavourite(long missionId,long userId);

	}
}
