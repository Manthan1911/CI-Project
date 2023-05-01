using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class BannerModel
	{
		public long BannerId { get; set; }

		public string? MediaName { get; set; }

		public string? MediaType { get; set; }

		public string? MediaPath { get; set; }

		[Required]
		public string? Title { get; set; }

		[Required]
		public string? Description { get; set; }

		[Required]
		public int? SortOrder { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		[Required]
		public IFormFile BannerImage { get; set; }
	}
}
