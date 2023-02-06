using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication3.Classes;
using WebApplication3.Migrations;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login LModel { get; set; }
 

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor contxt;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GoogleCaptchaService _captchaService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager, GoogleCaptchaService captchaService)
        {
            this.signInManager = signInManager;
            contxt = httpContextAccessor;
            _userManager = userManager;
            _captchaService = captchaService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var captchaResult = await _captchaService.VerifyToken(LModel.Token);
                if(!captchaResult)
                {
                    return Page();
                }
               
                var user = await _userManager.FindByEmailAsync(LModel.Email);
                if (user != null)
                {
                    var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                    var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                    var salt = protector.Unprotect(user.passwordSalt);
                    string pwdWithSalt = salt + LModel.Password;

                    var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, pwdWithSalt,
                    LModel.RememberMe, true);
                    if (identityResult.Succeeded)
                    {
                        HttpContext.Session.SetString("LoggedIn", LModel.Email);
                        string guid = Guid.NewGuid().ToString();
                        HttpContext.Session.SetString("AuthToken", guid);

                        var cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddDays(1);
                        cookieOptions.Path = "/";
                        Response.Cookies.Append("AuthToken", guid, cookieOptions);

                        //Create the security context
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Email, LModel.Email),

                        //This gives the new registered user a role of User
                        new Claim("Role", "User")
                        };
                        var i = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                        return RedirectToPage("Index");
                    }
                    else
                    {
                        if (identityResult.IsLockedOut)
                        {
                            TempData["FlashMessage.Type"] = "danger";
                            TempData["FlashMessage.Text"] = string.Format("Your account is locked out. Kindly wait for 1 minute and try again", LModel.Email);
                            ModelState.AddModelError("LockoutError", "Your account is locked out. Kindly wait for 1 minute and try again");
                            return Page();
                        }

                        TempData["FlashMessage.Type"] = "danger";
                        TempData["FlashMessage.Text"] = string.Format("invalid email or password for {0}", LModel.Email);
                        ModelState.AddModelError("", "Username or Password incorrect");
                    }
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("invalid email or password for {0}", LModel.Email);
                    ModelState.AddModelError("", "Username or Password incorrect");
                }

            }
            return Page();
        }

    }
}