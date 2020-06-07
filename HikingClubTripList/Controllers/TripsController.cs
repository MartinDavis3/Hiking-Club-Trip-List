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
            if (loggedInMember == null)
            {
                ViewData["LoggedInMemberName"] = "Log In";
                return View("Views/Home/Index.cshtml");
            }
            ViewData["LoggedInMemberName"] = loggedInMember.Name;

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
        //Method too large. Refactor DB operations as separate service, if time allows.
        {
            var loggedInMember = await _memberService.GetLoggedInMemberAsync();
            //ViewData["LoggedInMember"] = loggedInMember.MemberID;
            ViewData["LoggedInMemberName"] = loggedInMember.Name;
            if (id == null)
            {
                return NotFound();
            }
            int tripID = id ?? 0;
            var trip = await _tripService.GetTripDetailsAsync(tripID);
            if (trip == null)
            {
                return NotFound();
            }

            //Business logic.
            string leaderName = "Not Found";
            bool loggedInMemberIsLeader = false;
            bool loggedInMemberIsParticipant = false;
            List<string> participantNames = new List<string>();
            //Get leader and list of participants from the trip signups.
            //Also determine if logged in member is a leader or participant (or neither).
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
            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Title,Level,Distance,ElevationGain,Description,MaxParticipants")] Trip trip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if( !await _tripService.AddTripAsync(trip))
                    {
                        // Error ocurred during add.
                    }
                    CreateLeaderSignup(trip.TripID);
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
            int tripID = id ?? 0;
            var trip = await _tripService.GetTripOnlyAsync(tripID);
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
                    if (!await _tripService.UpdateTripAsync(trip) )
                    {
                        // Error ocurred updating trip.
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tripService.TripExistsAsync(trip.TripID))
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
            int tripID = id ?? 0;
            var trip = await _tripService.GetTripForDeleteAsync(tripID);

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
            if (!await _tripService.RemoveTripAsync(id))
            {
                // Error ocurred during delete.
            }
            return RedirectToAction(nameof(Index));
        }



        // Methods to handle trip Signups.
        // Should probably put these in a service layer, but due to time constraints, will leave them here for now.
        // Will refactor later, as time allows.

        // This is called directly by the signup button from the trip detail view.
        // Only the trip ID is passed in, the logged in member, found from the database, is signed up.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpForTrip(int tripId)
        {

            Signup signup = new Signup();
            signup.TripID = tripId;
            var member = await _memberService.GetLoggedInMemberAsync();
            signup.MemberID = member.MemberID;
            signup.AsLeader = false;

            try
            {
                if (ModelState.IsValid)
                {
                    if( !await _signupService.AddSignupAsync(signup) );
                    {
                        // Error ocurred while adding signup.
                    }
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Failed to sign up. Please try again.");
            }
            return RedirectToAction(nameof(Index));
        }

        // This is called directly by the withdrawl button from the trip detail view.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawFromTrip(int tripID)
        {
            var member = await _memberService.GetLoggedInMemberAsync();

            try
            {
                if ( member == null )
                {
                    int memberID = member.MemberID;
                    var signup = await _signupService.GetSignupAsync(tripID, memberID);

                    if (signup == null)
                    {
                        ModelState.AddModelError("", "Failed to withdraw [signup not found].");
                    }
                    if (!await _signupService.RemoveSignupAsync(signup))
                    {
                        // Error ocurred removing signup
                    }
               }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Failed to withdraw. Please try again.");
            }
            return RedirectToAction(nameof(Index));
        }

        // This is called directly by trip CREATE method.
        // Only the trip ID is passed in, the logged in member, found from the database, is signed up as leader.
        private async Task<IActionResult> CreateLeaderSignup(int tripID)
        {
            Signup leaderSignup = new Signup();

            var member = await _memberService.GetLoggedInMemberAsync();

            leaderSignup.MemberID = member.MemberID;
            leaderSignup.TripID = tripID;
            leaderSignup.AsLeader = true;

            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _signupService.AddSignupAsync(leaderSignup)) ;
                    {
                        // Error ocurred while adding signup.
                    }
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Failed to create a sign up. Please try again");
            }
        }

        // This is called directly by trip DELETE method.
        // Only the trip ID is passed in, all the associated signups are found and then deleted.
        private async Task<IActionResult> DeleteAllTripAssociatedSignups(int tripID)
        {
            var signups = _signupService.GetAllSignupsForTripAsync(tripID);
            try
            {
                if (signups == null)
                {
                    ModelState.AddModelError("", "Failed to withdraw [signup(s) not found].");
                }
                foreach (var s in signups)
                {
                    bool removeResult = _signupService.RemoveSignupAsync(s);
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Failed to remove signup(s). Please try again");
            }
        }

    }
}

