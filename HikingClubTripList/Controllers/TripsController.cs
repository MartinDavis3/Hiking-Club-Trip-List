using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingClubTripList.Data;
using HikingClubTripList.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;
using HikingClubTripList.Services;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HikingClubTripList.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IMemberService _memberService;
        private readonly ISignupService _signupService;

        private readonly string appErrorPath = "Views/Shared/AppError.cshtml";

        public TripsController(ITripService tripService, IMemberService memberService, ISignupService signupService)
        {
            _tripService = tripService;
            _memberService = memberService;
            _signupService = signupService;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            // First checks if a user is logged in. If not sends view to home page.
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);
            if (loggedInMember == null)
            {
                return View("Views/Home/Index.cshtml");
            }

            //Gets the list of trips with signups and names for processing.
            var trips = await _tripService.GetTripsListAsync();

            //Iterate through all trips a signups to get leader and number of participants.
            List<string> leaders = new List<string>();
            int numberOfParticipants;
            int spaces;
            List<string> spacesLeft = new List<string>();
            foreach (var t in trips)
            {
                numberOfParticipants = 0;
                foreach (var s in t.Signups)
                {
                    numberOfParticipants++;
                    if (s.AsLeader)
                    {
                        leaders.Add(s.Member.Name);
                    }
                }
                //Calculate spaces left, using "Full" if none.
                spaces = t.MaxParticipants - numberOfParticipants;
                if (spaces <= 0)
                {
                    spacesLeft.Add("Full");
                }
                else
                {
                    spacesLeft.Add(spaces.ToString());
                }
            }
            ViewData["LeadersList"] = leaders;
            ViewData["SpacesLeftList"] = spacesLeft;

            return View(trips);
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            Trip trip;
            // Set the error message, ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to get the trip details. Please try again.";
            if (id == null)
            {
                return View(appErrorPath);
            }
            int tripID = id ?? 0;
            try
            {
                trip = await _tripService.GetTripDetailsAsync(tripID);
                if (trip == null)
                {
                    return View(appErrorPath);
                }
            }
            catch
            {
                return View(appErrorPath);
            }
            // Clear error message
            ViewData["ErrorMessage"] = "";

            //Get leader and list of participants from the trip signups.
            //Also determine if logged in member is a leader or participant (or neither).
            string leaderName = "Not Found";
            bool loggedInMemberIsLeader = false;
            bool loggedInMemberIsParticipant = false;
            List<string> participantNames = new List<string>();

            foreach (var signup in trip.Signups)
            {
                if (signup.AsLeader)
                {
                    leaderName = signup.Member.Name;
                    if (loggedInMember.MemberID == signup.MemberID)
                    {
                        loggedInMemberIsLeader = true;
                    }
                }
                else
                {
                    participantNames.Add(signup.Member.Name);
                    if (loggedInMember.MemberID == signup.MemberID)
                    {
                        loggedInMemberIsParticipant = true;
                    }
                }
            }

            //Determine which actions will be available.
            bool tripIsFull = (participantNames.Count + 1 >= trip.MaxParticipants);
            bool includeDelete = false;
            bool includeEdit = false;
            bool includeWithdraw = false;
            bool includeSignup = false;
            if (loggedInMemberIsLeader)
            {
                includeDelete = true;
                includeEdit = true;
            }
            else if (loggedInMemberIsParticipant)
            {
                includeWithdraw = true;
            }
            else if (!tripIsFull)
            {
                includeSignup = true;
            }

            //Prepare view
            var tripDetailsView = new TripDetailsViewModel
            {
                Trip = trip,
                LeaderName = leaderName,
                ParticipantNames = participantNames,
                MemberName = loggedInMember.Name,
                IncludeDelete = includeDelete,
                IncludeEdit = includeEdit,
                IncludeWithdraw = includeWithdraw,
                IncludeSignup = includeSignup
            };

            return View(tripDetailsView);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            var loggedInMember = _memberService.GetLoggedInMember();
            PlaceLoggedInNameInViewData(loggedInMember);

            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Title,Level,Distance,ElevationGain,Description,MaxParticipants")] Trip trip)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            // Set error message ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to add the trip. Please try again.";

            if (ModelState.IsValid)
            {
                // Add trip to database and test result of action
                try
                {
                    if (!await _tripService.AddTripAsync(trip))
                    {
                        return View(appErrorPath);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View(appErrorPath);

                }

                // Add signup to this trip for logged in user as leader
                Signup leaderSignup = new Signup
                {
                    MemberID = loggedInMember.MemberID,
                    TripID = trip.TripID,
                    AsLeader = true
                };

                // Set error message ready if something goes wrong.
                ViewData["ErrorMessage"] = "An error ocurred trying to add the leader signup for this trip. Please check in the trip list if the trip has been created without a leader. If so, please delete the trip and try creating it again.";

                try
                {
                    if (!await _signupService.AddSignupAsync(leaderSignup))
                    {
                        return View(appErrorPath);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View(appErrorPath);
                }
                ViewData["ErrorMessage"] = "";
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            Trip trip;
            ViewData["ErrorMessage"] = "An error ocurred trying to get the trip to edit. Please try again.";
            if (id == null)
            {
                return View(appErrorPath);
            }
            int tripID = id ?? 0;
            try
            {
                trip = await _tripService.GetTripOnlyAsync(tripID);
                if (trip == null)
                {
                    return View(appErrorPath);
                }
            }
            catch
            {
                return View(appErrorPath);
            }
            ViewData["ErrorMessage"] = "";
            return View(trip);
        }

        // POST: Trips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripID,Date,Title,Level,Distance,ElevationGain,Description,MaxParticipants")] Trip trip)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            // Set error message ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to update the trip. Please try again.";

            if (id != trip.TripID)
            {
                return View(appErrorPath);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tripService.UpdateTripAsync(trip);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tripService.TripExistsAsync(trip.TripID))
                    {
                        return View(appErrorPath);
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ErrorMessage"] = "";
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            Trip trip;
            ViewData["ErrorMessage"] = "An error ocurred trying to get the trip to delete. Plaese try again.";
            if (id == null)
            {
                return View(appErrorPath);
            }
            int tripID = id ?? 0;
            try
            {
                trip = await _tripService.GetTripForDeleteAsync(tripID);
                if (trip == null)
                {
                    return View(appErrorPath);
                }
            }
            catch
            {
                return View(appErrorPath);

            }
            ViewData["ErrorMessage"] = "";
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            PlaceLoggedInNameInViewData(loggedInMember);

            // Set error message ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to delete the trip. Please try again.";

            try
            {
                if (!await _tripService.RemoveTripAsync(id))
                {
                    return View(appErrorPath);
                }
            }
            catch (DbUpdateException)
            {
                return View(appErrorPath);
            }

            // Delete signups.
            try
            {
                var signup = await _signupService.GetTripSignupAsync(id);
                while (signup != null)
                {
                    await _signupService.RemoveSignupAsync(signup);
                    signup = await _signupService.GetTripSignupAsync(id);
                } 
            }
            catch (DbUpdateException)
            {
                return View(appErrorPath);
            }

            ViewData["ErrorMessage"] = "";
            return RedirectToAction(nameof(Index));
        }

        // Methods to handle trip Signups.

        // This is called by the signup button from the trip detail view.
        // Only the trip ID is passed in, the logged in member, found from the database, is signed up.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpForTrip(int tripId)
        {
            var member = await _memberService.GetLoggedInMemberAsync();

            // Set error message ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to add the signup. Please try again.";

            Signup signup = new Signup
            {
                TripID = tripId,
                MemberID = member.MemberID,
                AsLeader = false
            };

            try
            {
                if (ModelState.IsValid)
                {
                    if( !await _signupService.AddSignupAsync(signup) )
                    {
                        return View(appErrorPath);
                    }
                }
            }
            catch (DbUpdateException)
            {
                return View(appErrorPath);
            }
            ViewData["ErrorMessage"] = "";
            return RedirectToAction(nameof(Index));
        }

        // This is called by the withdrawl button from the trip detail view.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawFromTrip(int tripID)
        {
            var member = await _memberService.GetLoggedInMemberAsync();

            // Set error message ready if something goes wrong.
            ViewData["ErrorMessage"] = "An error ocurred trying to add the signup. Please try again.";

            try
            {
                if ( member != null )
                {
                    int memberID = member.MemberID;
                    var signup = await _signupService.GetSignupAsync(tripID, memberID);

                    if (signup == null)
                    {
                        return View(appErrorPath);
                    }
                    if (!await _signupService.RemoveSignupAsync(signup))
                    {
                        return View(appErrorPath);
                    }
                }
            }
            catch (DbUpdateException)
            {
                return View(appErrorPath);
            }
            ViewData["ErrorMessage"] = "";
            return RedirectToAction(nameof(Index));
        }

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

