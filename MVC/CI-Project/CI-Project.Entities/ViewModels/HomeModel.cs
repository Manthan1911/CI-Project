using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
    public class HomeModel
    {
        public List<CountryModel> countries { get; set; }
        
        public List<CityModel> cities { get; set; }
        
        public List<ThemeModel> themes { get; set; }
        
        public List<SkillModel>  skills { get; set; }

        public User user { get; set; }

        public List<MissionModel> missions { get; set; } 

    }
}
