using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingClubTripList.Data;
using HikingClubTripList.Models;
using System.Diagnostics;

namespace HikingClubTripList.Controllers
{
    public class MembersController : Controller
    {
        private readonly ClubContext _context;

        public MembersController(ClubContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,Username,Password,Name,IsLoggedIn")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberID,Username,Password,Name,IsLoggedIn")] Member member)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberID))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }

        // This is the target of the Login nav tab.
        // It checks to see if a user is already logged in.
        // If so, it directs the view to the trips list page.
        // Otherwise is returns to the login form view.
        public IActionResult Login()
        {
            var member = _context.Members
                .FirstOrDefault(m => m.IsLoggedIn);
            if (member != null)
            {
                ViewData["LoggedInMemberName"] = member.Name;
                return View("Views/Home/Index.cshtml");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginSubmit([Bind("Username,Password")] Login login)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Username == login.Username && m.Password == login.Password);
            if (member == null)
            {
                ViewData["Message"] = "Username/Password not found. Please try again.";
                return View("Views/Members/Login.cshtml");
            }
            else
            {
                member.IsLoggedIn = true;
                _context.Update(member);
                await _context.SaveChangesAsync();
                ViewData["LoggedInMemberName"] = member.Name;
                return View("Views/Home/Index.cshtml");
            }
        }

        // This is the target of the logout nav bar tab, which immediately
        // logs the user out and sends the view back to the home page.
        public async Task<IActionResult> LogOut()
        {
            var member = _context.Members
                .FirstOrDefault(m => m.IsLoggedIn);
            if (member != null)
            {
                member.IsLoggedIn = false;
                _context.Update(member);
                await _context.SaveChangesAsync();
            }
            ViewData["LoggedInMemberName"] = "Guest";
            return View("Views/Home/Index.cshtml");
        }

    }
}
