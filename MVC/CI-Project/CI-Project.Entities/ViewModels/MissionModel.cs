using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations.Schema;

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


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public bool Status { get; set; }

        public string? OrganizationName { get; set; }


        public string? Availability { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public virtual City City { get; set; } = null!;


        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<FavouriteMission> FavouriteMissions { get; } = new List<FavouriteMission>();

        public virtual ICollection<GoalMission> GoalMissions { get; } = new List<GoalMission>();


        public virtual ICollection<MissionInvite> MissionInvites { get; } = new List<MissionInvite>();

        public virtual ICollection<MissionMedium> MissionMedia { get; } = new List<MissionMedium>();

        public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();


        public virtual MissionTheme Theme { get; set; } = null!;

        public string CoverImage { get; set; }
    }
}
