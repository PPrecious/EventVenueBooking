using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventVenueBooking.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Event>? Events { get; set; }
    }
}