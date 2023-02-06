using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        [BindProperty]
        public ForgetPassword FModel { get; set; }

        public void OnGet()
        {
        }
 //       protected void submitBtn_click(object sender, EventArgs e)
 //       {
//            MailMessage mail = new MailMessage();
//            mail.To.Add(email.Text.ToString().Trim());
//            mail.From = new MailAddress("ChuaandSpencers@gmail.com");
//            mail.Subject = "Forget Password";
//            mail.Body = "";
//            mail.IsBodyHtml = "";
//
 //           SmtpClient smtp = new SmtpClient();
//            smtp.Port =;
//
//        }
    }
}
