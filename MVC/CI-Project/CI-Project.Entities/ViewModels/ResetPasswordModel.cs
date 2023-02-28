using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
    public class ResetPasswordModel
    {
        [Required]
        [MinLength(8 , ErrorMessage = "Password should be minimum 8 characters long")]
        public string NewPassword { get; set; }
        
        [Required]
        [MinLength(8 , ErrorMessage = "Password should be minimum 8 characters long")]
        public string ConfirmPassword { get; set; }
    }
}
