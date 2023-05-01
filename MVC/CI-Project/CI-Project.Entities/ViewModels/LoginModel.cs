using CI_Project.Entities.DataModels;
using System.ComponentModel.DataAnnotations;


namespace CI_Project.Entities.ViewModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        //[RegularExpression("^[a-z]{1}[a-z\\.0-9]+@[a-z]+\\.+[a-z]{2,3}$", ErrorMessage = "Please enter valid e-mail address")]
        public string EmailId { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Please enter password of min 8 length")]
        public string Password { get; set; }

        public List<BannerModel>? BannerImages { get; set; }

    }
}
