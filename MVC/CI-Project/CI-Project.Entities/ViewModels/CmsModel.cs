using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class CmsModel
	{
		public long CmsPageId { get; set; }

		[Required]
		[MaxLength(255)]
		public string? Title { get; set; }

		public string? Description { get; set; }

		[Required]
		public string Slug { get; set; } = null!;

		public bool? Status { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }
	}
}
