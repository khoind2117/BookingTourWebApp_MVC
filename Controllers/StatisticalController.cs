using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTourWebApp_MVC.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticalController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public async Task<IActionResult> ThongKe()
        {

            var data= await (from flight in _context.Flights 
                             join  booking in _context.Bookings 
                             on flight.Id equals booking.FlightId
                             orderby (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice) ascending
                             select new StatisticalFlightSales()
                             {
                                 flightId = flight.Id,
                                 businessTickets = booking.BusinessTickets,
                                 businessPrice = flight.BusinessPrice,
                                 economyTickets = booking.EconomyTickets,
                                 economyPrice = flight.EconomyPrice,
                                 Sales = (booking.BusinessTickets* flight.BusinessPrice) + (booking.EconomyTickets*flight.EconomyPrice)
                             }).Take(3).ToListAsync();
            var data2 = await (from flight in _context.Flights
                              join booking in _context.Bookings
                              on flight.Id equals booking.FlightId
                              orderby (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice) descending
                              select new StatisticalFlightSales()
                              {
                                  flightId = flight.Id,
                                  businessTickets = booking.BusinessTickets,
                                  businessPrice = flight.BusinessPrice,
                                  economyTickets = booking.EconomyTickets,
                                  economyPrice = flight.EconomyPrice,
                                  Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                              }).Take(3).ToListAsync();
            return View(new StatisticalView
            {
                HighSales = data2,LowSales=data
            }) ;
        }
        public async Task<IActionResult> ThongKe2()
        {

            var data = await (from flight in _context.Flights
                              join booking in _context.Bookings
                              on flight.Id equals booking.FlightId
                              orderby (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice) descending
                              select new StatisticalFlightSales()
                              {
                                  flightId = flight.Id,
                                  businessTickets = booking.BusinessTickets,
                                  businessPrice = flight.BusinessPrice,
                                  economyTickets = booking.EconomyTickets,
                                  economyPrice = flight.EconomyPrice,
                                  Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                              }).Take(3).ToListAsync();
            return View(data);
        }
    }
}
