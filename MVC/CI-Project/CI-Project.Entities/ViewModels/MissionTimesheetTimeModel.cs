using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class MissionTimesheetTimeModel
	{

		public long TimesheetId { get; set; }

		[Required]
		public long? MissionId { get; set; }

		public long? UserId { get; set; }

		[Required]
		public DateTime? DateVolunteered { get; set; }

		[Required]
		[Range(0, 23)]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Hours")]
		public int? Hours { get; set; }

		[Required]
		[Range(0, 59)]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Minutes")]
		public int? Minutes { get; set; }

		[Required]
		[Display(Name = "Message")]
		[StringLength(80)]
		public string? Notes { get; set; }
		public string? ApprovalStatus { get; set; }

		public List<MissionModel>? TimeBasedMissions { get; set; }
	}
}
