using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication3.Migrations;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using System.Drawing;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;
        

        private IWebHostEnvironment _environment;

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            this.roleManager = roleManager;
        }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //for encrypting data
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                //for image
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload",
                        "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    RModel.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                //add salt to password hash
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                rng.GetBytes(saltByte);
                var salt = Convert.ToBase64String(saltByte);

                string pwdWithSalt = salt + RModel.Password;

                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    FullName = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.FullName)),
                    Email = System.Web.HttpUtility.HtmlEncode(RModel.Email),
                    Gender = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.Gender)),
                    MobileNo = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.MobileNo)),
                    passwordSalt = protector.Protect(System.Web.HttpUtility.HtmlEncode(salt)),
                    DeliveryAddress = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.DeliveryAddress)),
                    ImageURL = System.Web.HttpUtility.HtmlEncode(RModel.ImageURL),
                    Aboutme = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.AboutMe)),
                    CreditCard = protector.Protect(System.Web.HttpUtility.HtmlEncode(RModel.CreditCard))
                };


                //Create the user role if NOT exist
                IdentityRole role = await roleManager.FindByIdAsync("User");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("User"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }

                var result = await userManager.CreateAsync(user, pwdWithSalt);
                if (result.Succeeded)
                {
                    //Add users to User Role
                    result = await userManager.AddToRoleAsync(user, "User");

                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }





    }
}
