using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class UserProfileModel
	{
		public User? User { get; set; }

		public long UserId { get; set; }

		public string? EmailId { get; set; }


		[Required]
		[MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed")]
		public string? FirstName { get; set; }


		[Required]
		[MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed")]
		public string? LastName { get; set; }


		[Required]
		public long? PhoneNo { get; set; }


		[MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed")]
		public string? EmployeeId { get; set; }


		[MaxLength(255, ErrorMessage = "Maximum 255 characters are allowed")]
		public string? Title { get; set; }


		[MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed")]
		public string? Department { get; set; }


		[Required]
		public string? MyProfile { get; set; }


		public string? Avtar { get; set; }


		public string? WhyIVolunteer { get; set; }

		public string? LinkedIn { get; set; }

		public long? CountryId { get; set; }

		public long? CityId { get; set; }

		public byte? Availability { get; set; }

		public ICollection<City>? Cities { get; set; }

		public ICollection<Country>? Countries { get; set; }

		public ICollection<Skill>? Skills { get; set; }
		public virtual ICollection<UserSkill>? UserSkills { get; set; }


	}
}
