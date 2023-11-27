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
                return View(flightList[0]);
            }
        }
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
        public async Task<IActionResult> Create([Bind("Id,PlaneId,Departure,Destination,BusinessCapacity,EconomyCapacity,DepartureTime,BusinessPrice,EconomyPrice,UploadTime")] FlightViewModel flightViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightViewModel);
        }

        // GET: FlightViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            //if (id == null || _context.FlightViewModel == null)
            //{
            //    return NotFound();
            //}

            //var flightViewModel = await _context.FlightViewModel.FindAsync(id);
            //if (flightViewModel == null)
            //{
            //    return NotFound();
            //}
            //return View(flightViewModel);
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
                                    }).ToListAsync();
            return View(flightList[0]);
        }

        // POST: FlightViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaneId,Departure,Destination,BusinessCapacity,EconomyCapacity,DepartureTime,BusinessPrice,EconomyPrice,UploadTime")] FlightViewModel flightViewModel)
        {
            if (id != flightViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightViewModel);
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
            if (id == null || _context.FlightViewModel == null)
            {
                return NotFound();
            }

            var flightViewModel = await _context.FlightViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flightViewModel == null)
            {
                return NotFound();
            }

            return View(flightViewModel);
        }

        // POST: FlightViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FlightViewModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FlightViewModel'  is null.");
            }
            var flightViewModel = await _context.FlightViewModel.FindAsync(id);
            if (flightViewModel != null)
            {
                _context.FlightViewModel.Remove(flightViewModel);
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
