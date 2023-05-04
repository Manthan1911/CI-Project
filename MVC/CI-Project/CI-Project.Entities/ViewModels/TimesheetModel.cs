﻿using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class TimesheetModel
	{

		public long TimesheetId { get; set; }

		public long UserId { get; set; }

		public long MissionId { get; set; }

		public TimeSpan? Time { get; set; }

		public int? Action { get; set; }

		public DateTime DateVolunteered { get; set; }

		public string? Notes { get; set; }

		public string Status { get; set; } = null!;

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public virtual Mission Mission { get; set; } = null!;

		public virtual User User { get; set; } = null!;

	}
}
