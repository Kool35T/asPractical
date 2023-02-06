using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System.Net;
using WebApplication3.Classes;
using WebApplication3.Model;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDataProtection();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "FreshFarmMarket";
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Lockout.AllowedForNewUsers = true;
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    opts.Lockout.MaxFailedAccessAttempts = 3;
});

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options
=>
{
    options.Cookie.Name = "MyCookieAuth";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePages(context =>
{
    context.HttpContext.Response.ContentType = "text/plain";

    switch (context.HttpContext.Response.StatusCode)
    {
        case 404:
            context.HttpContext.Response.Redirect("/errors/404");
            break;
        default:
            context.HttpContext.Response.Redirect("/Errors/Error");
            break;
    }

    return Task.CompletedTask;

});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
