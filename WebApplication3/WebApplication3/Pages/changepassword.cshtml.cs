using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class changepasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public ChangePassword CModel { get; set; }
        public changepasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {


                var user = await _userManager.FindByEmailAsync(CModel.Email);

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var salt = protector.Unprotect(user.passwordSalt);
                //                string oldpwdWithSalt = salt + CModel.OldPassword;
                string newpwdWithSalt = salt + CModel.NewPassword;
                //
                //                var result = await _userManager.ChangePasswordAsync(user, oldpwdWithSalt, newpwdWithSalt);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, newpwdWithSalt);
                return Page();
            }
            return Page();
        }
        
    }
}
