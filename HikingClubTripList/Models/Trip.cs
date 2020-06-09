using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Models
{
    public enum Level
    {
        Beginner, Intermediate, Advanced
    }
    public class Trip
    {
        public int TripID { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "The title must be between 5 and 30 characters long, inclusive.")]
        [Required]
        [RegularExpression(@"^[A-Z]+.*[^\.\s]$", ErrorMessage = "The title must start with a capital letter, have no leading or trailing spaces and no terminating full stop.")]
        public string Title { get; set; }

        [Required]
        public Level Level { get; set; }

        [Display(Name = "Distance (km)")]
        [Range(0, 100, ErrorMessage = "Distance must be between 0 and 100 km.")]
        public double? Distance { get; set; }

        [Range(-5000, 5000, ErrorMessage = "Elevation gain must be between -5 000 and 5 000 m.")]
        [Display(Name = "Elevation Gain (m)")]
        public double? ElevationGain { get; set; }

        [StringLength(800, ErrorMessage = "The description must be a maximum of 800 characters long, including spaces.")]
        [RegularExpression(@"^[A-Z]+.*[^\s]$", ErrorMessage = "The description must start with a capital letter and have no leading or trailing spaces.")]
        public string Description { get; set; }

        [Range(2, 12, ErrorMessage = "The number of participants must be between 2 and 12.")]
        [Display(Name = "Maximum Participants")]
        [Required]
        public int MaxParticipants { get; set; }

        public ICollection<Signup> Signups { get; set; }
    }
}
