using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTourWebApp_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var changePasswordVM = new ChangePasswordViewModel();
            return View(changePasswordVM);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Có lỗi đã xảy ra. Vui lòng thử lại!";
                return RedirectToAction("ChangePassword", "User");
            }

            var user = await _userManager.GetUserAsync(User);
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordVM.OldPassword, changePasswordVM.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Có lỗi đã xảy ra. Vui lòng thử lại!";
                return RedirectToAction("ChangePassword", "User");
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ChangePassword", "User");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TicketHistory()
        {
            var userId = _userManager.GetUserId(User);

            var userBookings = await _context.Bookings
                .Where(b => b.AppUserId == userId)
                .Include(b => b.Flight) // Liên kết với bảng Flight để lấy thông tin chi tiết
                .ToListAsync();

            var ticketHistoryVMs = userBookings.Select(b => new TicketHistoryViewModel
            {
                FlightId = b.Flight.Id,
                Departure = b.Flight.Departure,
                Destination = b.Flight.Destination,
                DepartureTime = b.Flight.DepartureTime,
                BusinessTickets = b.BusinessTickets,
                EconomyTickets = b.EconomyTickets,
                TotalPrice = b.TotalPrice
            }).ToList();

            return View(ticketHistoryVMs);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TourHistory()
        {
            var userId = _userManager.GetUserId(User);

            var userTours = await _context.UserTours
                .Where(u => u.AppUserId == userId)
                .Include(u => u.Tour)
                .ToListAsync();

            var tourHistoryVMs = userTours.Select(t => new TourHistoryViewModel
            {
                Image = t.Tour.Image,
                Name = t.Tour.Name,
                Departure = t.Tour.Departure,
                Destination = t.Tour.Destination,
                DepartureDate = t.Tour.DepartureDate,
                ReturnDate = t.Tour.ReturnDate,
                Price = t.Tour.Price
            }).ToList();

            return View(tourHistoryVMs);
        }
    }
}