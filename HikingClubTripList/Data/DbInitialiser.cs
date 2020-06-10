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
                new Member{Username="user4", Password="pass4", Name="Konstantin Tsiolkovsky", IsLoggedIn=false},
                new Member{Username="user5", Password="pass5", Name="Satyendra Nath Bose", IsLoggedIn=false},
                new Member{Username="user6", Password="pass6", Name="Emmy Noether", IsLoggedIn=false},
                new Member{Username="user7", Password="pass7", Name="Lisa Cardinal", IsLoggedIn=false},
                new Member{Username="user8", Password="pass8", Name="Abdus Salam", IsLoggedIn=false},
                new Member{Username="user9", Password="pass9", Name="Paul Dirac", IsLoggedIn=false},
                new Member{Username="user10", Password="pass10", Name="Rosalind Franklin", IsLoggedIn=false},
                new Member{Username="user11", Password="pass11", Name="Kenichi Fukui", IsLoggedIn=false},
                new Member{Username="user12", Password="pass12", Name="Cyril Hinshelwood", IsLoggedIn=false},
                new Member{Username="user13", Password="pass13", Name="Gabriel García Márquez", IsLoggedIn=false},
                new Member{Username="user14", Password="pass14", Name="Léopold Senghor", IsLoggedIn=false},
                new Member{Username="user15", Password="pass15", Name="Raymond Bolomey", IsLoggedIn=false},
                new Member{Username="user16", Password="pass16", Name="Elizabeth Bloom", IsLoggedIn=false},
                new Member{Username="user17", Password="pass17", Name="Marie Laframboise", IsLoggedIn=false},
                new Member{Username="user18", Password="pass18", Name="Heather Plaskett", IsLoggedIn=false},
                new Member{Username="user19", Password="pass19", Name="Isaac Levison", IsLoggedIn=false},
                new Member{Username="user20", Password="pass20", Name="Edith Plamondon", IsLoggedIn=false}
            };
            foreach (Member m in members)
            {
                context.Members.Add(m);
            }
            context.SaveChanges();

            var trips = new Trip[]
            {
                new Trip{Date=DateTime.Parse("2020-6-12"), Title="Maligne Canyon", Level=Level.Beginner, Distance=7.5, ElevationGain=-200, Description="An easy but scenic walk downhill all the way! Start off with spectacular views into the canyon in this popular tourist spot with plenty of viewing areas. The path that the river has cut into the rock is very steep and narrow and a number of impressive waterfalls can be seen and heard in the canyon. The spray from the churning water supports abundant plant like on the upper reaches of the canyon walls. After a while, the crowds thin out and the canyon opens up. You continue following the river downstream past grassy bank strewn with wildflowers. As the river widens out, the current slackens somewhat and you make your way down towards a bridge that marks the end of the hike, just before the Maligne river joins the Athabasca river.", MaxParticipants=8 },
                new Trip{Date=DateTime.Parse("2020-7-28"), Title="Parker Ridge", Level=Level.Intermediate, Distance=6, ElevationGain=500, Description="This is a very nice hike at elevation, not too long but very steep fairly early on as you climb up a number of switchbacks. After a while, the path starts to flatten out as you approach the broad top of Parker ridge. You can observe the sparse, high altitude vegetation covering the stony ridge as you cross it to the other side. The imposing, snowy peak of Athabasca Mountain rises to your right and to the left you have a view of spectacular cliffs. Finally, as you reach the other side of the ridge, all your effort is rewarded by a great view of the Saskatchewan glacier and valley.", MaxParticipants=6 },
                new Trip{Date=DateTime.Parse("2020-6-25"), Title="Turbine Canyon", Level=Level.Advanced, Distance=16, ElevationGain=800, Description="This is a long but rewarding day hike with a steep section in the middle. The path starts of flat, following a river and, winding through luxurious forest. After several kilometers, the path branches and you take the left fork, heading away from the river on a slightly rising path. However, it is not long before the grade steepens and then it is a long climb up a number of widely spaced switchbacks, the vegetation gradually thinning as you reach higher elevations. Finally, all your effort is rewarded as you reach a plateau and you can explore the rocky slot of the unusual Turbine Canyon. From here the path makes a long loop of an Alpine meadow. Along the way there is a small waterfall and plunge pool where you can take a dip, as long as you don’t mind freezing cold water!", MaxParticipants=4 },
                new Trip{Date=DateTime.Parse("2020-8-15"), Title="Edith Cavell Meadows", Level=Level.Beginner, Distance=4.5, ElevationGain=100, Description="This is a nice, scenic stroll in the shadow of the imposing Edith Cavell Mountain. This high altitude meadow stays wet late into the season, so the beginning of August is the perfect time for this hike and the wildflowers should be at their best. As the hike is quite short, you have plenty of time to admire everything from the varied plant life beneath your feet, past the glacier perched above the small lake, to the imposing, rocky flanks of Edith Cavell.", MaxParticipants=8 },
                new Trip{Date=DateTime.Parse("2020-07-22"), Title="Sulphur Skyline", Level=Level.Intermediate, Distance=8, ElevationGain=700, Description="The first 2 km are done on a fairly wide path which rises gradually and continuously in the middle of a beautiful fir forest. There are not many views because you are still under the treeline. When you emerge in the alpine meadow, you are rewarded with a stunning view of the entire area. To the east you can see Fiddle River Valley and Mount Drinnan in the distance. To the west and south there are the Miette Range and Utopia Mountain. You can rest, enjoy the view and have lunch at the summit. Keep an eye on your sandwich since golden-mantled squirrels won’t be afraid of stealing it. At the end of your walk, you can relax your tired muscles at the Miette Hot Springs.", MaxParticipants=12 },
                new Trip{Date=DateTime.Parse("2020-08-04"), Title="Hummingbird Plume Lookout", Level=Level.Intermediate, Distance=12.5, ElevationGain=400, Description="This trail combines historical interest with beautiful scenery at the top. For the first part of the hike, you walk on the family-friendly Troll Falls hiking trail through an aspen forest until you reach Troll Falls. From Troll Falls to the lookout, it is uphill all the way to the top. Bears are quite numerous in the area, so be prepared to encounter one of these amazing animals. At the top, there are spectacular views across the Kananaskis Valley to Mount Lorette and the surrounding mountains of the Fisher Range. The Hummingbird Plume Fire Lookout was built around 1915 and it was used by German prisoners of war during World War II as a shelter while they were collecting firewood. Inside the badly damaged building you can find the names of some prisoners scratched into the wood.", MaxParticipants=10 },
                new Trip{Date=DateTime.Parse("2020-08-10"), Title="Cory Pass", Level=Level.Advanced, Distance=14, ElevationGain=1100, Description="This hike is only for the experienced hiker with no fear of heights and willing to use his/her hands to navigate the most difficult parts. You are going to do the hike clockwise and it is very steep for the first 4 km. Cory Pass is one of the most varied trails in Banff. This circuit takes in a number of different micro ecosystems from a dry ridge, through luxurious forest, to an arid pass. Not too be missed are the rock formations of the spectacular Gargoyle Valley! Wild flowers are abundant as the hike ascends through grasslands, affording amazing views of the surrounding mountains and back across Vermilion Lakes. The descent via Edith Pass is done on several kilometres of loose scree, which can be challenging, but the vistas on Mount Cory and Mount Louis are worth the effort.", MaxParticipants=6 },
                new Trip{Date=DateTime.Parse("2020-09-15"), Title="Eiffel Lake", Level=Level.Intermediate, Distance=12, ElevationGain=570, Description="The beginning of this hike is all steep switchbacks, but once you leave the forest behind you, you enter a different landscape with a rocky and barren slope. An impressive view of the Valley of the Ten Peaks makes you forget the abrupt start. A predominately flat trail leads to Eiffel Lake. The lake has a deep blue colour and is surrounded by glaciers, snow-capped mountains, huge rock piles and deep green forests. The end of September is the ideal season to admire the yellow larches and the grizzlies on the moraines, feeding for winter on the last dandelions.", MaxParticipants=8 },
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
                new Signup{TripID=4, MemberID=7, AsLeader=false },
                new Signup{TripID=4, MemberID=8, AsLeader=false },
                new Signup{TripID=4, MemberID=9, AsLeader=false },
                new Signup{TripID=4, MemberID=10, AsLeader=false },
                new Signup{TripID=4, MemberID=11, AsLeader=false },
                new Signup{TripID=4, MemberID=12, AsLeader=false },
                new Signup{TripID=4, MemberID=13, AsLeader=true },
                new Signup{TripID=4, MemberID=14, AsLeader=false },
                new Signup{TripID=4, MemberID=15, AsLeader=false },
                new Signup{TripID=4, MemberID=16, AsLeader=false },
                new Signup{TripID=4, MemberID=17, AsLeader=false },
                new Signup{TripID=4, MemberID=18, AsLeader=false },
                new Signup{TripID=5, MemberID=4, AsLeader=true },
                new Signup{TripID=6, MemberID=19, AsLeader=true },
                new Signup{TripID=7, MemberID=20, AsLeader=true },
                new Signup{TripID=8, MemberID=6, AsLeader=true }
            };
            foreach (Signup s in signups)
            {
                context.Signups.Add(s);
            }
            context.SaveChanges();

        }

    }

}
