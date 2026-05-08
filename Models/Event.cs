using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVenueBooking.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event Name is required")]
        [StringLength(100, ErrorMessage = "Event Name cannot exceed 100 characters")]
        public string EventName { get; set; } = null!;

        [Required(ErrorMessage = "Event Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [ForeignKey("Venue")]
        public int? VenueId { get; set; }

        public Venue? Venue { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Prevent past dates
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventDate < DateTime.Now)
            {
                yield return new ValidationResult(
                    "Event date cannot be in the past.",
                    new[] { nameof(EventDate) }
                );
            }
        }
    }
}