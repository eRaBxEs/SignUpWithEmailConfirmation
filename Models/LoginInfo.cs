using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignupWithMailConfirmation.Models
{
    public class LoginInfo : CommonProperties
    {
        [Key]
        public int UserInfoId { get; set; }

        [Display(Name="EmailAddress")]
        [Required(ErrorMessage="Email Address is required")]
        [EmailAddress(ErrorMessage="Invalid Email Address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage="Username is required")]
        [StringLength(16, ErrorMessage="Must be between 5 and 16", MinimumLength=5)]
        public string Username { get; set; }

        [Required(ErrorMessage="Password is required")]
        [StringLength(255, ErrorMessage="Must be between 5 and 255 characters", MinimumLength=5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage="Confirm Password is required")]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public bool IsmailConfirmed { get; set; }
        
    }
}