using System.ComponentModel.DataAnnotations;

namespace FirstConsoleApp.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Please Enter Your Email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email!")]
        
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Password!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
