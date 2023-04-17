using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class MissionTimesheetGoalModel
	{
		public long TimesheetId { get; set; }

		[Required]
		public long? MissionId { get; set; }
		public string? MissionName { get; set; }

		public long? UserId { get; set; }

		[Required]
		public DateTime? DateVolunteered { get; set; }

		[StringLength(80)]
		public string? Notes { get; set; }

		[Required]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Action input.")]
		public int? Action { get; set; }

		public string? ApprovalStatus { get; set; }

		public List<Mission>? Missions { get; set; }
		public List<Timesheet>? TimesheetData{ get; set; }
	}
}
