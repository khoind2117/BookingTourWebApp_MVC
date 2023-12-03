using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingTourWebApp_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    // Người dùng bị khóa tài khoản
                    return View("Lockout");
                }
                else
                {
                    // Hiển thị lỗi khi người dùng nhập sai mật khẩu
                    ModelState.AddModelError("", "Đăng nhập thất bại, vui lòng kiểm tra lại!");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterAsync(string? returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin") || !await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var userByUsername = await _userManager.FindByNameAsync(registerViewModel.UserName);

                // Kiểm tra Username
                if (userByUsername != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại, vui lòng chọn tên đăng nhập khác!");
                    return View(registerViewModel);

                }

                // Tạo tài khoản
                if (userByUsername == null)
                {
                    var user = new AppUser 
                    { 
                        UserName = registerViewModel.UserName,
                        FullName = registerViewModel.FullName,
                        PhoneNumber = registerViewModel.PhoneNumber,
                        Email = registerViewModel.Email,
                    };
                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                    if (result.Succeeded)
                    {
                        #region Role
                        await _userManager.AddToRoleAsync(user, "User"); // Gán role Admin/User
                        #endregion
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // Nếu tạo không thành công sẽ trả về
                // Có thể là do mật khẩu chưa đúng quy định, cần làm rõ hơn cho người dùng
                ModelState.AddModelError("Password", "Tạo tài khoản không thành công");
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
