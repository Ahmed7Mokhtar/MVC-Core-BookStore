using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FirstConsoleApp.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Please Enter Your First Name!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Your Last Name!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Password!")]
        [Display(Name = "Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password Doesn't Match!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Confirm Your Password!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
