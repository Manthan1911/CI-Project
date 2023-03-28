using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class StoryModel
	{
		public long StoryId { get; set; }

		public long UserId { get; set; }

		public long MissionId { get; set; }
		
		public string? StoryTitle { get; set;}

		public string? StoryDescription { get; set;}

		public string? Status { get;set; }

		public DateTime? PublishedAt { get; set; }
		
		public DateTime CreatedAt { get; set; }
		public virtual User? User { get; set; }

		public virtual Mission? Mission { get; set; }

		public virtual ICollection<StoryMedium>? StoryMedia { get; set; }

		public string? CoverImage { get; set; }
	}
}
