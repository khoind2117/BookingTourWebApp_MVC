using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.Models;
using BookingTourWebApp_MVC.ViewModels;

namespace BookingTourWebApp_MVC.Controllers
{
    public class TicketManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<ActionResult<IEnumerable<Booking>>> Index(string? searchValue, string searchType, string? departure, string? destination, DateTime departureTime, DateTime bookingTime)
        {
            var query = _context.Bookings.Include(a => a.AppUser).Include(b => b.Flight).AsQueryable();

            if (!string.IsNullOrEmpty(departure))
            {
                query = query.Where(f => f.Flight.Departure == departure);
            }

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(f => f.Flight.Destination == destination);
            }

            if (departureTime.ToString().Substring(0, 8) != "1/1/0001" && departureTime.ToString().Substring(0, 10) != "01/01/0001")
            {
                query = query.Where(f => f.Flight.DepartureTime.Year == departureTime.Year && f.Flight.DepartureTime.Month == departureTime.Month && f.Flight.DepartureTime.Day == departureTime.Day);
            }

            if (bookingTime.ToString().Substring(0, 8) != "1/1/0001" && bookingTime.ToString().Substring(0, 10) != "01/01/0001")
            {
                query = query.Where(f => f.BookingTime.Year == bookingTime.Year && f.BookingTime.Month == bookingTime.Month && f.BookingTime.Day == bookingTime.Day);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (searchType == "flightId")
                {
                    query = query.Where(f => f.FlightId.ToString().Contains(searchValue));
                }
                if (searchType == "ticketId")
                {
                    query = query.Where(f => f.Id.ToString().Contains(searchValue));
                }
            }

            var ticketList = await query.Select(f => new Booking {
                Id = f.Id,
                AppUser = f.AppUser,
                AppUserId = f.AppUserId,
                FlightId = f.FlightId,
                Flight = f.Flight,
                BusinessTickets = f.BusinessTickets,
                EconomyTickets = f.EconomyTickets,
                TotalPrice = f.TotalPrice,
                BookingTime = f.BookingTime
            }).ToListAsync();
            return View(ticketList);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.AppUser)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }


        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
