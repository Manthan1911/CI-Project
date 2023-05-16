using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
    public class NotificationSettingsModel
    {

        public long UserId { get; set; }

        public bool RecommendMission { get; set; }

        public bool RecommendStory { get; set; }

        public bool VolunteeringHour { get; set; }

        public bool VolunteeringGoal { get; set; }

        public bool MyStory { get; set; }

        public bool NewMission { get; set; }

        public bool NewMessage { get; set; }

        public bool MissionApplication { get; set; }

        public bool News { get; set; }

        public bool Mail { get; set; }

        public virtual User User { get; set; } = null!;

    }
}
