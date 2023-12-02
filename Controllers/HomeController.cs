using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookingTourWebApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
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

    }
}