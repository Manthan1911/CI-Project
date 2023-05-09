using CI_Project.Entities.DataModels;
namespace CI_Project.Entities.ViewModels
{
    public class MissionModel
    {

        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public bool Status { get; set; }

        public string? OrganizationName { get; set; }
        public string? OrganizationDetails { get; set; }


        public string? Availability { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        public DateTime? RegisterationDeadline { get; set; }

        public virtual City City { get; set; } = null!;


        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<FavouriteMission> FavouriteMissions { get; set; } = new List<FavouriteMission>();

        public virtual ICollection<GoalMission> GoalMissions { get; set; } = new List<GoalMission>();
        public virtual GoalMission? GoalMission { get; set; }
        public virtual ICollection<MissionInvite> MissionInvites { get; set; } = new List<MissionInvite>();

        public virtual ICollection<MissionMedium> MissionMedia { get; set; } = new List<MissionMedium>();

        public virtual ICollection<MissionRating> MissionRatings { get; set; } = new List<MissionRating>();
        public virtual ICollection<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();


        public virtual MissionTheme Theme { get; set; } = null!;

        public string? CoverImage { get; set; }

        public User? user { get; set; }
        public long? CurrentUserId { get; set; }

        public int countOfRatingsByPeople { get; set; }
        
        public int sumOfRating { get; set; }

        public int avgRating { get; set; }

        public int isMissionFavourite { get; set; }

        public long? seatsLeft { get; set; }
        
        public long? totalSeats { get; set; }

        public int isMissionApplied { get; set; }

        public int GoalAchieved { get; set; }

        public int AvgGoal { get; set; }

        public long totalVolunteers { get; set; }

        public bool isMissionAppliedByCurrentUser { get; set; }
        public bool isMissionApplicationPending { get; set; }
        public bool isMissionApplicationDeclined { get; set; }
    }
}
