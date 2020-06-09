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
