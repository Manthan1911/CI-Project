using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        public List<BannerModel>? BannerImages { get; set; }

    }
}
