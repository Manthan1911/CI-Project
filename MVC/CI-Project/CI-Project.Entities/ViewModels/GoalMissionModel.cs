using CI_Project.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class GoalMissionModel
	{
		public long MissionId { get; set; }

		[Required]
		public long MissionCity { get; set; }

		[Required]
		public long MissionCountry { get; set; }

		[Required]
		public long MissionThemeId { get; set; }

		[Required]
		public string Title { get; set; } = null!;

		[Required]
		public string? ShortDescription { get; set; }

		[Required]
		public string? Description { get; set; }

		[Required]
		public string OrganizationName { get; set; } = null!;

		public string? OrganizationDetails { get; set; }

		[Required]
		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public long? TotalSeats { get; set; }

		public DateTime? RegistrationDeadline { get; set; }

		[Required]
		public string? Availability { get; set; }

		public long MissionGoalId { get; set; }

		[Required]
		[StringLength(50)]
		public string? GoalObjective { get; set; }

		[Required]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Action input.")]
		public int? GoalValue { get; set; }

		[Required]
		public bool? IsActive { get; set; }
		public bool? FetchMissionImages { get; set; }
		public bool? FetchMissionDocuments { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? UpdatedAt { get; set; }

		public DateTimeOffset? DeletedAt { get; set; }

		public List<City> Cities { get; set; } = new();
		public List<Country> Countries { get; set; } = new();
		public List<MissionTheme> Themes { get; set; } = new();
		public List<Skill> Skills { get; set; } = new();
		public List<MissionMedium> MissionMedia { get; set; } = new();
		public List<MissionDocument> MissionDocument { get; set; } = new();
		public List<MissionSkills> MissionSkill { get; set; } = new();


        public List<int>? SelectedSkills { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Documents { get; set; }
    }
}
