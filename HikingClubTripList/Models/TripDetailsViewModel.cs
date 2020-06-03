using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Models
{
    public class TripDetailsViewModel
    {
        public Trip Trip { get; set; }
        public string LeaderName { get; set; }
        public List<string> ParticipantNames { get; set; }
        public string MemberName { get; set; }
        public bool IncludeDelete { get; set; }
        public bool IncludeEdit { get; set; }
        public bool IncludeWithdraw { get; set; }
        public bool IncludeSignup { get; set; }
    }
}
