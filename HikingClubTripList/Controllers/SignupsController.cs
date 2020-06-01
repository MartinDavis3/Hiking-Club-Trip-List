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
    public class SignupsController : Controller
    {
        private readonly ClubContext _context;

        public SignupsController(ClubContext context)
        {
            _context = context;
        }

        // GET: Signups
        public async Task<IActionResult> Index()
        {
            var clubContext = _context.Signups.Include(s => s.Member).Include(s => s.Trip);
            return View(await clubContext.ToListAsync());
        }

        // GET: Signups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signup = await _context.Signups
                .Include(s => s.Member)
                .Include(s => s.Trip)
                .FirstOrDefaultAsync(m => m.SignupID == id);
            if (signup == null)
            {
                return NotFound();
            }

            return View(signup);
        }

        // GET: Signups/Create
        public async Task<IActionResult> Create(int? TripID)
        {
            var member =  await _context.Members
                .FirstOrDefaultAsync(m => m.IsLoggedIn);

            //The last parameter in SelectList is the item which will be selected by default
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name", member.MemberID);
            ViewData["TripID"] = new SelectList(_context.Trips, "TripID", "Title", TripID);
            return View();
        }

        // POST: Signups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripID,MemberID,AsLeader")] Signup signup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(signup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name", signup.MemberID);
            ViewData["TripID"] = new SelectList(_context.Trips, "TripID", "Title", signup.TripID);
            return View(signup);
        }

        // This is called directly by the signup button from the trip detail view.
        // Only the trip ID is passed in, the logged in member, found from the database, is signed up.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpForTrip([Bind("TripID")] Signup signup)
        {
            
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.IsLoggedIn);

            signup.MemberID = member.MemberID;

            string fragment = "/" + Convert.ToString(signup.TripID);

            if (ModelState.IsValid)
            {
                _context.Add(signup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Trips", fragment);
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name", signup.MemberID);
            ViewData["TripID"] = new SelectList(_context.Trips, "TripID", "Title", signup.TripID);
            return View(signup);
        }

        // GET: Signups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signup = await _context.Signups.FindAsync(id);
            if (signup == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name", signup.MemberID);
            ViewData["TripID"] = new SelectList(_context.Trips, "TripID", "Title", signup.TripID);
            return View(signup);
        }

        // POST: Signups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SignupID,TripID,MemberID,AsLeader")] Signup signup)
        {
            if (id != signup.SignupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignupExists(signup.SignupID))
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
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name", signup.MemberID);
            ViewData["TripID"] = new SelectList(_context.Trips, "TripID", "Title", signup.TripID);
            return View(signup);
        }

        // GET: Signups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signup = await _context.Signups
                .Include(s => s.Member)
                .Include(s => s.Trip)
                .FirstOrDefaultAsync(m => m.SignupID == id);
            if (signup == null)
            {
                return NotFound();
            }

            return View(signup);
        }

        // POST: Signups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signup = await _context.Signups.FindAsync(id);
            _context.Signups.Remove(signup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignupExists(int id)
        {
            return _context.Signups.Any(e => e.SignupID == id);
        }
    }
}
