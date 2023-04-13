using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
	public class ChangePasswordModel
	{
		public User? User { get; set; }

		[Required]
		public long UserId { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "Old Password should be minimum 8 characters long")]
		public string? OldPassword { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "New Password should be minimum 8 characters long")]
		public string? NewPassword { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "Confirm Password should be minimum 8 characters long")]
		public string? ConfirmPassword { get; set; }
	}
}
