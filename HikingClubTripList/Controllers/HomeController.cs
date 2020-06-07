using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HikingClubTripList.Models;
using HikingClubTripList.Data;
using HikingClubTripList.Services;

namespace HikingClubTripList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberService _memberService;

        public HomeController(ILogger<HomeController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            return View();
        }

        public IActionResult Privacy()
        {
            var loggedInMember = _memberService.GetLoggedInMember();
            PlaceLoggedInNameInViewData(loggedInMember);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Routine is repeated from trip controller rather than putting it in a service.
        // Chose to prioritise principle of separation of concerns over DRY.
        private void PlaceLoggedInNameInViewData(Member loggedInMember)
        {
            if (loggedInMember == null)
            {
                ViewData["LoggedInMemberName"] = "Log In";
            }
            else
            {
                ViewData["LoggedInMemberName"] = loggedInMember.Name;
            }
        }

    }
}
