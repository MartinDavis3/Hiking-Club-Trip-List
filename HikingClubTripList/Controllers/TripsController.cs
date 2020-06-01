using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingClubTripList.Data;
using HikingClubTripList.Models;

namespace HikingClubTripList.Controllers
{
    public class TripsController : Controller
    {
        private readonly ClubContext _context;

        public TripsController(ClubContext context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trips.ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["LoggedInMember"] = LoggedInMember();
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Signups)
                    .ThenInclude(s => s.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TripID == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Title,Level,Distance,ElevationGain,Description,MaxParticipants")] Trip trip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(trip);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again");
            }
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripID,Date,Title,Level,Distance,ElevationGain,Description,MaxParticipants")] Trip trip)
        {
            if (id != trip.TripID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.TripID))
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
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .FirstOrDefaultAsync(m => m.TripID == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.TripID == id);
        }


        // Methods to add Signups necessary to sign up participants on trips

        // This is called directly by the signup button from the trip detail view.
        // Only the trip ID is passed in, the logged in member, found from the database, is signed up.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpForTrip([Bind("TripID")] Signup signup)
        {

            signup.MemberID = LoggedInMember();

            try
            {
                if (ModelState.IsValid)
            {
                _context.Add(signup);
                await _context.SaveChangesAsync();
            }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Failed to sign up. Please try again");
            }
            return RedirectToAction(nameof(Index));
        }

        // For use inside controller. Does not need async methods as it will be called from an async Task.
        private int LoggedInMember()
        {
            var member = _context.Members
                .FirstOrDefault(m => m.IsLoggedIn);
            if (member == null)
            {
                return 0;
            }
            return member.MemberID;
        }


    }
}

