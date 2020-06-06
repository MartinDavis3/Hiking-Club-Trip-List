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
            ViewData["LoggedInMemberName"] = "Log In";
            return View("Views/Home/Index.cshtml");
        }

    }
}
