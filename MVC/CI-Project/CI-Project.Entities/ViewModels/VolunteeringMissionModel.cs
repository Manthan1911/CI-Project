using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
    public  class VolunteeringMissionModel
    {
        public MissionModel MissionModel { get; set; }

        public User user { get; set; }  
    }
}
