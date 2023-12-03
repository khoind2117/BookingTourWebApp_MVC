using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingTourWebApp_MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string flightsJson = HttpContext.Session.GetString("FlightsData");

            // Ngăn cho người dùng truy cập bằng cách sử dụng url trực tiếp mà không thông qua form
            if (string.IsNullOrEmpty(flightsJson))
            {
                ViewBag.ErrorMessage = TempData["NoFlightsMessage"] as string;
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<Flight> flights = JsonConvert.DeserializeObject<IEnumerable<Flight>>(flightsJson);

            // Xóa dữ liệu từ Session sau khi đã sử dụng để không lưu trữ lâu dài
            //HttpContext.Session.Remove("FlightsData");

            return View(flights);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookFlight(int flightId)
        {
            var user = await _userManager.GetUserAsync(User);
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id ==flightId);
            if(flight == null)
            {
                return NotFound();
            }

            var bookFlightVM = new BookFlightViewModel
            {
                appUser = user,
                flight = flight,                
            };

            return View(bookFlightVM);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBookFlight(int flightId, string appUserId, int businessTickets, int economyTickets, decimal totalPrice)
        {
            // Lấy thông tin chuyến bay, user, giá vé từ cơ sở dữ liệu dựa trên ID hoặc thông tin nhận được từ form
            var appUser = await _userManager.FindByIdAsync(appUserId);
            var flight = await _context.Flights.FindAsync(flightId); // Thay flightId bằng ID chuyến bay

            if(businessTickets <= 0 && economyTickets <= 0)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại";
                return RedirectToAction("Index", "Booking");
            }

            // Kiểm tra nếu số vé đặt vượt quá số vé còn lại trong chuyến bay
            if (businessTickets > flight.BusinessCapacity || economyTickets > flight.EconomyCapacity)
            {
                TempData["ErrorMessage"] = "Số vé đặt vượt quá số vé còn lại trong chuyến bay! Vui lòng đặt lại";
                return RedirectToAction("Index", "Booking");
            }

            var booking = new Booking
            {
                AppUser = appUser,
                Flight = flight,
                BusinessTickets = businessTickets,
                EconomyTickets = economyTickets,
                TotalPrice = totalPrice,
                BookingTime = DateTime.UtcNow
            };

            // Cập nhật lại số vé còn lại trong chuyến bay
            flight.BusinessCapacity -= businessTickets;
            flight.EconomyCapacity -= economyTickets;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt vé thành công!";
            return RedirectToAction("Index", "Booking");
        }
    }
}