namespace EventVenueBooking.Models
{
    public class BookingDetailsView
    {
        public int BookingId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
