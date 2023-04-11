using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class StoryModel
	{
		public long StoryId { get; set; }

		public long UserId { get; set; }

		[Required]
		public long MissionId { get; set; }

		[Required]
		[MaxLength(255)]
		public string? StoryTitle { get; set;}

		[Required]
		[MaxLength(40000)]
		public string? StoryDescription { get; set;}

		public string? Status { get;set; }

		public DateTime? PublishedAt { get; set; }

		public DateTime CreatedAt { get; set; }
		public virtual User? User { get; set; }

		public virtual Mission? Mission { get; set; }

		public virtual ICollection<StoryMedium>? StoryMedia { get; set; }
		public virtual ICollection<MissionModel>? Missions { get; set; }
		public virtual ICollection<Story>? StoriesOfCurrentUser { get; set; }

		public string? CoverImage { get; set; }

		public string? VideoUrl { get; set; }

		public int IsStoryDraft { get; set; }

		public long? Views { get; set; }
	}
}
