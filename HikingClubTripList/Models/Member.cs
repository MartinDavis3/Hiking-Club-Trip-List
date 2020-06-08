using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Models
{
    public class Member
    {
        // The model is for internal use only.
        // Data is entered directly to the database by administrator.
        // No user input is received.
        public int MemberID { get; set; }

        [StringLength(12, MinimumLength = 5 ) ]
        [Required]
        public string Username { get; set; }

        [StringLength(12, MinimumLength = 5 ) ]
        [Required]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 5 ) ]
        [Required]
        public string Name { get; set; }

        public bool IsLoggedIn { get; set; }

        public ICollection<Signup> Signups { get; set; }

    }
}
