using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace WebApplication3.Classes
{
    public class CustomAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            // Check if user has the required role
            var hasRole = context.HttpContext.User.IsInRole("Admin");
            if (!hasRole)
            {
                context.Result = new RedirectResult("/errors/401");
            }
        }
    }
}
