using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
	public class ForgetPassword
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
