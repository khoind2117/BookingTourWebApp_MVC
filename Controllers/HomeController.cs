using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BookingTourWebApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var latestFlights = _context.Flights
            .OrderByDescending(f => f.DepartureTime)
            .Take(3)
            .ToList();

            return View(latestFlights);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SearchByDepartureAndDestinationAndDate(string departure, string destination, DateTime departureTime)
        {
            IEnumerable<Flight> flights = await _context.Flights.Where(f => f.Departure == departure
                                                && f.Destination == destination
                                                && f.DepartureTime.Date == departureTime)
                                                .OrderBy(f => f.DepartureTime.TimeOfDay)
                                                .ToListAsync();
            if (flights == null)
            {
                TempData["NoFlightsMessage"] = "No flights available for the given criteria.";
                return RedirectToAction("Index", "Booking");
            }

            string flightsJson = JsonConvert.SerializeObject(flights);
            HttpContext.Session.SetString("FlightsData", flightsJson); // Lưu chuỗi JSON vào Session

            return RedirectToAction("Index", "Booking");
        }
    }
}