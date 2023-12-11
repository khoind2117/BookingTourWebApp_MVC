using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Helpers;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.Services.Implementations;
using BookingTourWebApp_MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPhotoService, PhotoService>();

#region Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
#endregion

#region ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Cấu hình quy định mật khẩu
    options.Password.RequiredLength = 6; // Chiều dài tối thiểu là 6 ký tự
    options.Password.RequireNonAlphanumeric = false; // Không yêu cầu ký tự không phải chữ cái
    options.Password.RequireUppercase = true; // Yêu cầu chữ hoa
    options.Password.RequireLowercase = true; // Yêu cầu chữ thường
    options.Password.RequireDigit = true; // Yêu cầu số
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    //options.SignIn.RequireConfirmedAccount = true; yêu cầu người dùng xác nhận tài khoản
});
#endregion

#region External Login (Google, Facebook)
builder.Services.AddAuthentication()
    .AddFacebook(facebookOptions =>
    {
        //facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        //facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        facebookOptions.AppId = "895663108626722";
        facebookOptions.AppSecret = "6345a4626bc26ec8de33547b30d2a146";
        //facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
        facebookOptions.AccessDeniedPath = "/";
    })
    .AddGoogle(googleOptions =>
    {
        //googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        //googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        googleOptions.ClientId = "779825606353-11hrtlfgf3kk7nusjc9vj02fmjeu7lhv.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
    });
#endregion

#region Cookie & Session
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout cho session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
