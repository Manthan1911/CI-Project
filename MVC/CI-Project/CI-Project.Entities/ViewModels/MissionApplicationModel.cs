using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class MissionApplicationModel
	{
		public long MissionApplicationId { get; set; }

		public long? MissionId { get; set; }

		public string? MissionTitle { get; set; }

		public long? UserId { get; set; }
		public string? UserName { get; set; }

		public DateTime AppliedAt { get; set; }

		public string ApprovalStatus { get; set; } = null!;

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public virtual Mission? Mission { get; set; }

		public virtual User? User { get; set; }
	}
}
