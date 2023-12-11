using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using FlightBooking.ViewModel;
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

        #region External Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string? returnurl = null)
        {
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("Error");
                }
                var user = new AppUser
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnurl);
                    }
                }
                ModelState.AddModelError("Email", "User already exists");
            }
            ViewData["ReturnUrl"] = returnurl;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnurl = null, string remoteError = null)
        {
            // Xử lý lỗi nếu có
            if (remoteError != null)
            {
                // Xử lý lỗi ở đây (ví dụ: hiển thị một trang lỗi)
                ModelState.AddModelError(string.Empty, "Error from external provider");
                return View("Login");
            }
            // Nhận thông tin đăng nhập từ nhà cung cấp
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Xử lý lỗi nếu không nhận được thông tin đăng nhập
                return RedirectToAction("Login");
            }

            // Kiểm tra xem người dùng đã đăng ký tài khoản trong ứng dụng của bạn hay chưa
            // Cụ thể là đã điền đầy đủ thông tin trong trang ExternalLoginConfirmation
            // Nếu chưa điền thông tin sẽ chuyển đến trang đó điền và tạo tài khoản
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Người dùng đã đăng nhập thành công, thực hiện các xử lý cần thiết
                // Chuyển hướng về trang chính hoặc returnUrl
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnurl);
            }
            else
            {
                ViewData["ReturnUrl"] = returnurl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnurl = null)
        {
            var redirect = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnurl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            return Challenge(properties, provider);
        }
        #endregion
    }
}
