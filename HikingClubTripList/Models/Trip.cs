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

        [StringLength(30, MinimumLength = 5)]
        [Required]
        public string Title { get; set; }

        [Required]
        public Level Level { get; set; }

        [Display(Name = "Distance (km)")]
        [Range(0, 100)]
        public double? Distance { get; set; }

        [Range(-5000, 5000)]
        [Display(Name = "Elevation Gain (m)")]
        public double? ElevationGain { get; set; }

        [StringLength(800)]
        public string Description { get; set; }

        [Range(2, 12)]
        [Display(Name = "Maximum Participants")]
        [Required]
        public int MaxParticipants { get; set; }

        public ICollection<Signup> Signups { get; set; }
    }
}
