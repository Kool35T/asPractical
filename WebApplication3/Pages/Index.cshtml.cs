using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize(Roles ="User,Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contxt;
        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _contxt = httpContextAccessor;
            _userManager = userManager;
        }
        public ApplicationUser User { get; set; } = new();
        public async Task OnGetAsync()
        {
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");

            var userdata = await _userManager.GetUserAsync(HttpContext.User);

            User = new ApplicationUser()
            {
                FullName = protector.Unprotect(userdata.FullName),
                Email = userdata.Email,
                Gender = protector.Unprotect(userdata.Gender),
                MobileNo = protector.Unprotect(userdata.MobileNo),
                ImageURL = userdata.ImageURL,
                DeliveryAddress = protector.Unprotect(userdata.DeliveryAddress),
                Aboutme = protector.Unprotect(userdata.Aboutme)
            };
            


        }

    }
}