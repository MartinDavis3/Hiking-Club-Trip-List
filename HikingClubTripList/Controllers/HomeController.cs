using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HikingClubTripList.Models;
using HikingClubTripList.Data;

namespace HikingClubTripList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClubContext _context;

        public HomeController(ILogger<HomeController> logger, ClubContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var loggedInMember = LoggedInMember();
            if (loggedInMember == null)
            {
                ViewData["LoggedInMemberName"] = "Guest";
            }
            ViewData["LoggedInMemberName"] = loggedInMember.Name;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Member LoggedInMember()
        {
            var member = _context.Members
                .FirstOrDefault(m => m.IsLoggedIn);
            return member;
        }
    }
}
