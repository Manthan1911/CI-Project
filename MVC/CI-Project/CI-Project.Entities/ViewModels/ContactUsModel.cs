using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class ContactUsModel
	{
		public long ContactUsId { get; set; }

		public long UserId { get; set; }

		[Required]
		[MaxLength(255)]
		public string? Subject { get; set; }

		[Required]
		[MaxLength(60000)]
		public string? Message { get; set; }

		public string? Reply { get; set; }

		public bool? Status { get; set; }

		public virtual User User { get; set; } = null!;
		public string? UserName { get; set; } = null!;
		public string? EmailAddress { get; set; } = null!;

	}
}
