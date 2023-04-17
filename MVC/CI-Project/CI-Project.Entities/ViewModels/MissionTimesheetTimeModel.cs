﻿using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class MissionTimesheetTimeModel
	{

		public long TimesheetId { get; set; }

		[Required]
		public long MissionId { get; set; }

		public string? MissionName { get; set; }

		public long UserId { get; set; }

		[Required]
		public DateTime? DateVolunteered { get; set; } = DateTime.Now;

		[Required]
		[Range(0, 23)]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Hours")]
		public int? Hours { get; set; }

		[Required]
		[Range(0, 59)]
		[RegularExpression("([0-9]+)", ErrorMessage = "Invalid Minutes")]
		public int? Minutes { get; set; }

		public TimeSpan? Time { get; set; }

		public string? Notes { get; set; }
		public string? ApprovalStatus { get; set; }

		public List<Mission>? Missions { get; set; }
		public List<Timesheet>? TimesheetData { get; set; }
	}
}
