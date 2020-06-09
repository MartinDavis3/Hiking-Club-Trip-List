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
using HikingClubTripList.Services;

namespace HikingClubTripList.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // The ASP.NET Core MVC with EF Core - tutorial series (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-3.1)
        // was used to help setup the initial code and database structure for this App. This was then extensively modified to fullfil the
        // particular business model required for the project application.

        // Routines handling login and logout.

        // This is the target of the Login nav tab.
        public async Task<IActionResult> Login()
        {
            var member = await _memberService.GetLoggedInMemberAsync();

            if (member != null)
            {
                ViewData["LoggedInMemberName"] = member.Name;
                return View("Views/Home/Index.cshtml");
            }
            ViewData["LoggedInMemberName"] = "Log In";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginSubmit([Bind("Username,Password")] Login login)
        {
            var member = await _memberService.GetValidMemberAsync(login);
            if (member == null)
            {
                ViewData["Message"] = "Username/Password not found. Please try again.";
                ViewData["LoggedInMemberName"] = "Log In";
                return View("Views/Members/Login.cshtml");
            }
            else
            {
                member.IsLoggedIn = true;
                if (await _memberService.ChangeMemberLoginStateAsync(member))
                {
                    ViewData["LoggedInMemberName"] = member.Name;
                    return View("Views/Home/Index.cshtml");
                }
                else
                {
                    ViewData["Message"] = "Error logging In. Please try again.";
                    ViewData["LoggedInMemberName"] = "Log In";
                    return View("Views/Members/Login.cshtml");
                }
            }
        }

        // This is the target of the logout nav bar tab.
        public async Task<IActionResult> LogOut()
        {
            var member = await _memberService.GetLoggedInMemberAsync();
            if (member != null)
            {
                member.IsLoggedIn = false;
                if (!await _memberService.ChangeMemberLoginStateAsync(member))
                {
                    ViewData["ErrorMessage"] = "An error ocurred trying to logout. Please try again.";
                    ViewData["LoggedInMemberName"] = member.Name;
                    return View("Views/Shared/AppError.cshtml");
                }
            }
            ViewData["LoggedInMemberName"] = "Log In";
            return View("Views/Home/Index.cshtml");
        }

    }
}
