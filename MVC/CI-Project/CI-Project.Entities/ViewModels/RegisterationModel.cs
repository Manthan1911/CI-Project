﻿using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
    public class RegisterationModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid phone no!")]
        public string PhoneNo { get; set; }

        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

        public List<BannerModel>? BannerImages { get; set; } 
        
    }
}
