using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTourWebApp_MVC.Data;
using BookingTourWebApp_MVC.ViewModels;
using BookingTourWebApp_MVC.Models;
using System.Numerics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingTourWebApp_MVC.Controllers
{
    public class FlightManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightManagementController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region GET flights
        // GET: FlightViewModels
        public async Task<ActionResult<IEnumerable<FlightViewModel>>> Index(string? id, string? departure, string? destination, DateTime departureTime)
        {

            var query = _context.Flights.Include(f => f.Plane).AsQueryable();

            if (!string.IsNullOrEmpty(departure))
            {
                query = query.Where(f => f.Departure == departure);
            }

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(f => f.Destination == destination);
            }

            if (departureTime.ToString().Substring(0,8) != "1/1/0001" && departureTime.ToString().Substring(0, 10) != "01/01/0001")
            {
                query = query.Where(f => f.DepartureTime.Year == departureTime.Year && f.DepartureTime.Month == departureTime.Month && f.DepartureTime.Day == departureTime.Day);
            }

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(f => f.Id.ToString().Contains(id));
            }


            var flightList = await query.Select(f => new FlightViewModel
            {
                Id = f.Id,
                PlaneId = f.PlaneId,
                PlaneName = f.Plane.Name,
                Departure = f.Departure,
                Destination = f.Destination,
                BusinessCapacity = f.BusinessCapacity,
                EconomyCapacity = f.EconomyCapacity,
                DepartureTime = f.DepartureTime,
                BusinessPrice = f.BusinessPrice,
                EconomyPrice = f.EconomyPrice,
                UploadTime = f.UploadTime,
                BusinessBooked = f.Bookings.Where(b => b.FlightId == f.Id).Sum(b => b.BusinessTickets),
                EconomyBooked = f.Bookings.Where(c => c.FlightId == f.Id).Sum(c => c.EconomyTickets)
            }).ToListAsync();

            return View(flightList);
        }
        #endregion
        //=========================================================================
        //GET: FlightViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var query = _context.Flights.Include(f => f.Plane).Include(c => c.Bookings).AsQueryable();
            query = query.Where(f => f.Id == id);
            var flightDetail = await query.Select(f => new FlightViewModel
            {
                Id = f.Id,
                PlaneId = f.PlaneId,
                PlaneName = f.Plane.Name,
                Departure = f.Departure,
                Destination = f.Destination,
                BusinessCapacity = f.BusinessCapacity,
                EconomyCapacity = f.EconomyCapacity,
                DepartureTime = f.DepartureTime,
                BusinessPrice = f.BusinessPrice,
                EconomyPrice = f.EconomyPrice,
                UploadTime = f.UploadTime,
                BusinessBooked = f.Bookings.Where(b => b.FlightId == f.Id).Sum(b => b.BusinessTickets),
                EconomyBooked = f.Bookings.Where(c => c.FlightId == f.Id).Sum(c => c.EconomyTickets)
            }).FirstOrDefaultAsync();
            return View(flightDetail);
        }
         //GET: FlightViewModels/Create
        public IActionResult Create()
        {

            ViewBag.planeList = _context.Planes.FromSqlInterpolated($"select * from dbo.Plane").ToList();

            return View();
        }
        // POST: FlightViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel addFlightRequest)
        {

            if (addFlightRequest.DepartureTime <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(addFlightRequest.DepartureTime), "Thời gian bay phải sau thời điểm upload.");
            }
            if (addFlightRequest.DepartureTime.Year > DateTime.Now.Year + 1)
            {
                ModelState.AddModelError(nameof(addFlightRequest.DepartureTime), "Thời gian bay chỉ cách thời điểm hiện tại tối đa 1 năm.");
            }
            if (addFlightRequest.Departure == addFlightRequest.Destination)
            {
                ModelState.AddModelError(nameof(addFlightRequest.Departure), "Nơi xuất phát và nơi đến không được trùng nhau.");
                ModelState.AddModelError(nameof(addFlightRequest.Destination), "Nơi xuất phát và nơi đến không được trùng nhau.");
            }

            if (ModelState.IsValid)
            {
                var flight = new Flight()
                {
                    PlaneId = addFlightRequest.PlaneId,
                    Departure = addFlightRequest.Departure,
                    Destination = addFlightRequest.Destination,
                    BusinessCapacity = addFlightRequest.BusinessCapacity,
                    EconomyCapacity = addFlightRequest.EconomyCapacity,
                    DepartureTime = addFlightRequest.DepartureTime,
                    BusinessPrice = addFlightRequest.BusinessPrice,
                    EconomyPrice = addFlightRequest.EconomyPrice,
                    UploadTime = DateTime.Now
                };

                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.planeList = _context.Planes.FromSqlInterpolated($"select * from dbo.Plane").ToList();
            return View(addFlightRequest);
        }

        // GET: FlightViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var flightList = await (from f in _context.Flights
                                    join m in _context.Planes
                                    on f.PlaneId equals m.Id
                                    where f.Id.ToString().Equals(id)
                                    select new FlightViewModel()
                                    {
                                        Id = f.Id,
                                        PlaneId = f.PlaneId,
                                        PlaneName = m.Name,
                                        Departure = f.Departure,
                                        Destination = f.Destination,
                                        BusinessCapacity = f.BusinessCapacity,
                                        EconomyCapacity = f.EconomyCapacity,
                                        DepartureTime = f.DepartureTime,
                                        BusinessPrice = f.BusinessPrice,
                                        EconomyPrice = f.EconomyPrice,
                                        UploadTime = f.UploadTime
                                    }).FirstOrDefaultAsync();
            ViewBag.planeList = _context.Planes.FromSqlInterpolated($"select * from dbo.Plane").ToList();
            return View(flightList);
        }

        // POST: FlightViewModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaneName,PlaneId,Departure,Destination,BusinessCapacity,EconomyCapacity,DepartureTime,BusinessPrice,EconomyPrice,UploadTime")] FlightViewModel flightViewModel)
        {
            if (id != flightViewModel.Id)
            {
                return NotFound();
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound("Flight not found");
            }
            if (flightViewModel.DepartureTime <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(flightViewModel.DepartureTime), "Thời gian bay phải sau thời điểm upload.");
            }
            if (flightViewModel.DepartureTime.Year > DateTime.Now.Year + 1)
            {
                ModelState.AddModelError(nameof(flightViewModel.DepartureTime), "Thời gian bay chỉ cách thời điểm hiện tại tối đa 1 năm.");
            }
            if (flightViewModel.Departure == flightViewModel.Destination)
            {
                ModelState.AddModelError(nameof(flightViewModel.Departure), "Nơi xuất phát và nơi đến không được trùng nhau.");
                ModelState.AddModelError(nameof(flightViewModel.Destination), "Nơi xuất phát và nơi đến không được trùng nhau.");
            }
            flight.PlaneId = flightViewModel.PlaneId;
            flight.Departure = flightViewModel.Departure;
            flight.Destination = flightViewModel.Destination;
            flight.BusinessCapacity = flightViewModel.BusinessCapacity;
            flight.EconomyCapacity = flightViewModel.EconomyCapacity;
            flight.DepartureTime = flightViewModel.DepartureTime;
            flight.BusinessPrice = flightViewModel.BusinessPrice;
            flight.EconomyPrice = flightViewModel.EconomyPrice;
            flight.UploadTime = flightViewModel.UploadTime;

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(flightViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightlExists(flightViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.planeList = _context.Planes.FromSqlInterpolated($"select * from dbo.Plane").ToList();
            return View(flightViewModel);
        }

        // GET: FlightViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }
            var flightList = await (from f in _context.Flights
                                    join m in _context.Planes
                                    on f.PlaneId equals m.Id
                                    where f.Id.Equals(id)
                                    select new FlightViewModel()
                                    {
                                        Id = f.Id,
                                        PlaneId = f.PlaneId,
                                        PlaneName = m.Name,
                                        Departure = f.Departure,
                                        Destination = f.Destination,
                                        BusinessCapacity = f.BusinessCapacity,
                                        EconomyCapacity = f.EconomyCapacity,
                                        DepartureTime = f.DepartureTime,
                                        BusinessPrice = f.BusinessPrice,
                                        EconomyPrice = f.EconomyPrice,
                                        UploadTime = f.UploadTime,
                                        BusinessBooked = f.Bookings.Where(b => b.FlightId == f.Id).Sum(b => b.BusinessTickets),
                                        EconomyBooked = f.Bookings.Where(c => c.FlightId == f.Id).Sum(c => c.EconomyTickets)
                                    }).FirstOrDefaultAsync();
            if (flightList == null)
            {
                return NotFound();
            }
            return View(flightList);
        }

        // POST: FlightViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flights == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FlightViewModel'  is null.");
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightlExists(int id)
        {
          return (_context.Flights?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
