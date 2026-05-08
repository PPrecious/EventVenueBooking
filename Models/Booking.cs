using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVenueBooking.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        public int VenueId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public bool IsValidDateRange => EndDate > StartDate;

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }
    }
}