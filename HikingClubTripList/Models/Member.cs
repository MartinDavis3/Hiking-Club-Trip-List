using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Models
{
    public class Member
    {
        public int MemberID { get; set; }

        [StringLength(12, MinimumLength = 5 ) ]
        // Must not start with a digit
        [RegularExpression(@"^[^0-9]")]
        [Required]
        public string Username { get; set; }

        [StringLength(12, MinimumLength = 5 ) ]
        [Required]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 5 ) ]
        // Must start with capital letter and have no digits
        [RegularExpression(@"^[A-Z]+[^0-9]")]
        [Required]
        public string Name { get; set; }

        public ICollection<Signup> Signups { get; set; }

    }
}
