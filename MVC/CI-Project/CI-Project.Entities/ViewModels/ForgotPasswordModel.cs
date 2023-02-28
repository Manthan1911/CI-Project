using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [RegularExpression("^[a-z]{1}[a-z0-9]+@[a-z]+\\.+[a-z]{2,3}$", ErrorMessage = "Please enter valid e-mail address")]
        public string EmailId { get; set; }

    }
}
