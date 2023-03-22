using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class CommentModel
	{
		public long CommentId { get; set; }

		public long UserId { get; set; }

		public long MissionId { get; set; }

		public string ApprovalStatus { get; set; } = null!;

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public virtual Mission Mission { get; set; } = null!;

		public virtual User User { get; set; } = null!;
		public string CommentText { get; set; } = null!;

	}
}
