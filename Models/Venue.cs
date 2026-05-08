using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventVenueBooking.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters")]
        public string VenueName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 100000, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }

        [Url(ErrorMessage = "Please enter a valid image URL")]
        public string? ImageUrl { get; set; }

        // Navigation
        public ICollection<Event>? Events { get; set; }
    }
}