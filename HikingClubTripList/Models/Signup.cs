using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Models
{
    //Link between Trip and Member tables, data will not be entered directly by the user.
    public class Signup
    {
        public int SignupID { get; set; }

        public int TripID { get; set; }

        public int MemberID { get; set; }

        public bool AsLeader { get; set; }

        public Trip Trip { get; set; }

        public Member Member { get; set; }
    }
}
