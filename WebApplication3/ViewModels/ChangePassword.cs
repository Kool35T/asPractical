using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
	public class ChangePassword
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}", ErrorMessage = "Minimum 12 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
