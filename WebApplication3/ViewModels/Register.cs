using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
//        [RegularExpression(@"(^[A-Za-z]{3,16})([ ]{0,1})([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})", ErrorMessage = "Invalid full name ")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Invalid Phone Number")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"(\d{1,3}.)?.+\s(\d{6})$", ErrorMessage = "Invalid Address(has to be in singapore)")]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [RegularExpression(@"^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Invalid Credit Card Number(Visa))")]
        public string CreditCard { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}", ErrorMessage = "Minimum 12 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        [MaxLength(50)]
        public string? ImageURL { get; set; }
    }
}
