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

namespace BookingTourWebApp_MVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region GET flights
        // GET: FlightViewModels
        public async Task<ActionResult<IEnumerable<FlightViewModel>>> Index(string? id)
        {
            //return _context.Flights != null ? 
            //            View(await _context.Flights.ToListAsync()) :
            //            Problem("Entity set 'ApplicationDbContext.FlightViewModel'  is null.");
            if (id == null)
            {
                var flightList = await (from f in _context.Flights
                                  join m in _context.Planes
                                  on f.PlaneId equals m.Id
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
                                  }).ToListAsync();
                return View(flightList);
            }
            else
            {
                var flightList = await (from f in _context.Flights
                                  join m in _context.Planes
                                  on f.PlaneId equals m.Id
                                  where f.Id.ToString().Contains(id)
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
                                  }).ToListAsync();
                return View(flightList);
            }
        }
        #endregion
        //=========================================================================
        // GET: FlightViewModels/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.FlightViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    var flightViewModel = await _context.FlightViewModel
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (flightViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(flightViewModel);
        //}
        //=========================================================================
        // GET: FlightViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlightViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlaneId,Departure,Destination,BusinessCapacity,EconomyCapacity,DepartureTime,BusinessPrice,EconomyPrice,UploadTime")] Flight addFlightRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addFlightRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(flightList);
        }

        // POST: FlightViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (!FlightViewModelExists(flightViewModel.Id))
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
                                        UploadTime = f.UploadTime
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

        private bool FlightViewModelExists(int id)
        {
          return (_context.FlightViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
