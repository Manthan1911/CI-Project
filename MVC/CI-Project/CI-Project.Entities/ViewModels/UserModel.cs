using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class UserModel
	{
		public long UserId { get; set; }

		public string? EmailId { get; set; }
		
		[Required]
		public string? FirstName { get; set; }

		[Required]
		public string? LastName { get; set; }

		[Required]
		public long? PhoneNo { get; set; }

		[Required]
		public string? Password { get; set; }

		public string? EmployeeId { get; set; }

		public string? Title { get; set; }

		public string? Department { get; set; }

		[Required]
		public string? MyProfile { get; set; }

		public string? WhyIVolunteer { get; set; }

		public string? LinkedIn { get; set; }

		public long? CountryId { get; set; } 

		public long? CityId { get; set; }
		
		[Required]
		public bool? Status { get; set; }

		public virtual ICollection<MissionInvite>? missionInvitesTo { get; set; }

		public virtual ICollection<MissionInvite>? missionInvitesFrom { get; set; }

		public virtual ICollection<UserSkill>? UserSkills { get; set; }
	}
}
