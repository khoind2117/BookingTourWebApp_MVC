using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            
            return RedirectToAction("ThongKe");
        }
        [Route("/Statistcal/")]
        [Route("/Statistical/ThongKe")]
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
            var now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, now.Month, daysInMonth);
            var data3 = await (from flight in _context.Flights
                               join booking in _context.Bookings
                               on flight.Id equals booking.FlightId
                               where (flight.DepartureTime >= firstDay && flight.DepartureTime <= lastDay)
                               orderby flight.Id
                               select new StatisticalFlightSales()
                               {
                                   flightId = flight.Id,
                                   Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                               }).ToListAsync();
            var dataSaleMonth = await (from flight in _context.Flights
                               join booking in _context.Bookings
                               on flight.Id equals booking.FlightId
                               where (flight.DepartureTime >= firstDay && flight.DepartureTime <= lastDay)
                               select new StatisticalFlightSales()
                               {
                                   flightId = flight.Id,
                                   Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                               }).ToListAsync();
            var tongChuyenMonth = dataSaleMonth.Count();
            var tongSaleMonth = dataSaleMonth.Sum(dt=>dt.Sales);
            var data4 = new StatisticalTotalSaleMonth()
            {
                CountFlight = tongChuyenMonth,
                TotalSales = tongSaleMonth,
            };

            var dt1 = await (from user in _context.Users
                               join booking in _context.Bookings
                               on user.Id equals booking.AppUserId
                               select new StatisticalUser()
                               {
                                   Id=user.Id,
                                   FullName=user.FullName,
                                   businessTickets = booking.BusinessTickets,
                                   economyTickets = booking.EconomyTickets,
                                   TotalTickets= booking.BusinessTickets + booking.EconomyTickets
                               }).ToListAsync();
            var data5 = dt1.GroupBy(u => new { u.Id, u.FullName }).Select(u => new StatisticalUser()
            {
                Id = u.Key.Id,
                FullName = u.Key.FullName,
                TotalTickets = u.Sum(a => a.TotalTickets)
            }).OrderByDescending(u => u.TotalTickets).Take(3).ToList();
            var dt2 = await (from user in _context.Users
                            join booking in _context.Bookings
                            on user.Id equals booking.AppUserId
                            select new StatisticalUser()
                            {
                                Id = user.Id,
                                FullName = user.FullName,
                                businessTickets = booking.BusinessTickets,
                                economyTickets = booking.EconomyTickets,
                                TotalTickets = booking.BusinessTickets + booking.EconomyTickets
                            }).ToListAsync();
            var data6 = dt2.GroupBy(u => new { u.Id, u.FullName }).Select(u => new StatisticalUser()
            {
                Id = u.Key.Id,
                FullName = u.Key.FullName,
                TotalTickets = u.Sum(a => a.TotalTickets)
            }).OrderBy(u => u.TotalTickets).Take(3).ToList();
            return View(new StatisticalView
            {
                HighSales = data2,LowSales=data,
                AllSaleInMonth = data3, TotalSaleInMonth=data4,
                UserHaveHighTicket=data5,UserHaveLowTicket=data6,
            }) ;
        }
        

    }
}
