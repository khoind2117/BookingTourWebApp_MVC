using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;
using BookingTourWebApp_MVC.ViewModels.VMofStatistical;
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
        public async Task<IActionResult> HandleSearchStatistical(string firstDate, string lastDate)
        {
            var firstD = DateTime.Parse(firstDate);
            var lastD = DateTime.Parse(lastDate);
            var dataNow = await (from flight in _context.Flights
                                 join booking in _context.Bookings
                                 on flight.Id equals booking.FlightId
                                 where (flight.DepartureTime >= firstD && flight.DepartureTime <= lastD)
                                 select new StatisticalFlightSales()
                                 {
                                     flightId = flight.Id,
                                     Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                                 }).ToListAsync();
            var data3 = dataNow.GroupBy(f => new { f.flightId }).Select(c => new StatisticalFlightSales()
            {
                flightId = c.Key.flightId,
                Sales = c.Sum(f => f.Sales)
            }).ToList();
            return PartialView("HandleSearchStatistical", data3);
        }
        public async Task<IActionResult> HandleTotalSaleSearchStatistical(string firstDate, string lastDate)
        {
            var firstD = DateTime.Parse(firstDate);
            var lastD = DateTime.Parse(lastDate);
            var dataNow = await (from flight in _context.Flights
                                 join booking in _context.Bookings
                                 on flight.Id equals booking.FlightId
                                 where (flight.DepartureTime >= firstD && flight.DepartureTime <= lastD)
                                 select new StatisticalFlightSales()
                                 {
                                     flightId = flight.Id,
                                     Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                                 }).ToListAsync();
            var data3 = dataNow.GroupBy(f => new { f.flightId }).Select(c => new StatisticalFlightSales()
            {
                flightId = c.Key.flightId,
                Sales = c.Sum(f => f.Sales)
            }).ToList();
            var sumSaleMonth = (from item in data3 select item.Sales).Sum();
            var sumFlightMonth = (from item in data3 select item.flightId).Distinct().Count();
            var data4 = new StatisticalTotalSaleMonth()
            {
                Id = 1,
                CountFlight = sumFlightMonth,
                TotalSales = sumSaleMonth,
            };
            return PartialView(data4);
        }
        [Route("/Statistcal/")]
        [Route("/Statistical/ThongKe")]
        public async Task<IActionResult> ThongKe()
        {
            
            //Doanh thu, số vé của chuyến bay thấp nhất
            var data = await (from flight in _context.Flights 
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
            //doanh thu, số vé của chuyến bay cao nhất
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
            //doanh thu của các chuyến bay tháng hiện tại
            var now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, now.Month, daysInMonth);
            var dataNow = await (from flight in _context.Flights
                               join booking in _context.Bookings
                               on flight.Id equals booking.FlightId
                               where (flight.DepartureTime >= firstDay && flight.DepartureTime <= lastDay)
                               
                               select new StatisticalFlightSales()
                               {
                                   flightId = flight.Id,
                                   Sales = (booking.BusinessTickets * flight.BusinessPrice) + (booking.EconomyTickets * flight.EconomyPrice)
                               }).ToListAsync();
            var data3 = dataNow.GroupBy(f => new { f.flightId }).Select(c => new StatisticalFlightSales()
            {
                flightId = c.Key.flightId,
                Sales = c.Sum(f => f.Sales)
            }).ToList();
            //doanh thu tổng của chuyến bay tháng hiện tại
            var sumSaleMonth = (from item in data3 select item.Sales).Sum();
            var sumFlightMonth = (from item in data3 select item.flightId).Distinct().Count();
            var data4 = new StatisticalTotalSaleMonth()
            {
                Id = 1,
                CountFlight = sumFlightMonth,
                TotalSales = sumSaleMonth,
            };
            //Khách hàng có số lượng vé đặt ít nhất
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
            //Khách hàng có số lượng vé đặt nhiều nhất
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
            var data7 = await (from user in _context.Users
                               join booking in _context.Bookings
                               on user.Id equals booking.AppUserId
                               select new StatisticalBookingTime()
                               {
                                   Id =user.Id,
                                   FullName=user.FullName,
                                   Gmail=user.Email,
                                   phoneNumber=user.PhoneNumber,
                                   bookingTime=booking.BookingTime,
                               }).OrderByDescending(u=>u.bookingTime).Take(10).ToListAsync();
            return View("Statistical",new StatisticalView
            {
                HighSales = data2,LowSales=data,
                AllSaleInMonth = data3, TotalSaleInMonth=data4,
                UserHaveHighTicket=data5,UserHaveLowTicket=data6,
                HistoryBookingTime=data7,
            }) ;
        }
        

    }
}
