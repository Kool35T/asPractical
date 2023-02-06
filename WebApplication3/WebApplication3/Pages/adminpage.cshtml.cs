using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Classes;

namespace WebApplication3.Pages
{
    [CustomAuth]
    public class adminpageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
