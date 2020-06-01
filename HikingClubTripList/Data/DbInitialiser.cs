using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HikingClubTripList.Models;

namespace HikingClubTripList.Data
{
    public static class DbInitialiser
    {
        public static void Initialize(ClubContext context)
        {
            // Create the database if it does not already exist.
            context.Database.EnsureCreated();

            // Check for any members (if so, DB has already been seeded so return.
            if (context.Members.Any())
            {
                return;
            }

            var members = new Member[]
            {
                new Member{Username="user1", Password="pass1", Name="Alice Jones", IsLoggedIn=false},
                new Member{Username="user2", Password="pass2", Name="Bob Smith", IsLoggedIn=false},
                new Member{Username="user3", Password="pass3", Name="Charlie McDonald", IsLoggedIn=false},
                new Member{Username="user4", Password="pass4", Name="David Harrison", IsLoggedIn=false},
                new Member{Username="user5", Password="pass5", Name="Elizabeth Bloom", IsLoggedIn=true},
                new Member{Username="user6", Password="pass6", Name="Fred Donaldson", IsLoggedIn=false},
                new Member{Username="user7", Password="pass7", Name="Gladys Davis", IsLoggedIn=false},
                new Member{Username="user8", Password="pass8", Name="Heather Plaskett", IsLoggedIn=false},
                new Member{Username="user9", Password="pass9", Name="Isaac Levison", IsLoggedIn=false},
                new Member{Username="user10", Password="pass10", Name="Jerry Plamondon", IsLoggedIn=false}
            };
            foreach (Member m in members)
            {
                context.Members.Add(m);
            }
            context.SaveChanges();

            var trips = new Trip[]
            {
                new Trip{Date=DateTime.Parse("2020-6-12"), Title="Malign Canyon", Level=Level.Beginner, Distance=7.5, ElevationGain=-200, Description="An easy but scenic walk downhill all the way!", MaxParticipants=8 },
                new Trip{Date=DateTime.Parse("2020-7-28"), Title="Parker Ridge", Level=Level.Intermediate, Distance=6, ElevationGain=500, Description="A nice hike at elevation. Not too long but steep. It is worth the effort though, with great views of the Saskatchewan glacier.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce nisl elit, mollis utlibero quis, accumsan dapibus massa.", MaxParticipants=6 },
                new Trip{Date=DateTime.Parse("2020-6-25"), Title="Turbine Canyon", Level=Level.Advanced, Distance=16, ElevationGain=800, Description="A long day hike with a steep section in the middle. Rewarded with views of the unusual Turbine Canyon.", MaxParticipants=4 },
                new Trip{Date=DateTime.Parse("2020-8-15"), Title="Edith Cavell Meadows", Level=Level.Beginner, Distance=4.5, ElevationGain=100, Description="A nice, but scenic stroll.", MaxParticipants=8 },
            };
            foreach (Trip t in trips)
            {
                context.Trips.Add(t);
            }
            context.SaveChanges();

            var signups = new Signup[]
            {
                new Signup{TripID=1, MemberID=1, AsLeader=true },
                new Signup{TripID=1, MemberID=5, AsLeader=false },
                new Signup{TripID=1, MemberID=2, AsLeader=false },
                new Signup{TripID=2, MemberID=6, AsLeader=true },
                new Signup{TripID=2, MemberID=7, AsLeader=false },
                new Signup{TripID=3, MemberID=3, AsLeader=true },
                new Signup{TripID=4, MemberID=8, AsLeader=true },
                new Signup{TripID=4, MemberID=9, AsLeader=false }
            };
            foreach (Signup s in signups)
            {
                context.Signups.Add(s);
            }
            context.SaveChanges();

        }

    }

}
