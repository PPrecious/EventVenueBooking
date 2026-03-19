using EventVenueBooking.Data;
using EventVenueBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEaseVenueBooking.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue);

            return View(await bookings.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Events = _context.Events.ToList();
            ViewBag.Venues = _context.Venues.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            bool conflict = _context.Bookings.Any(b =>
                b.VenueId == booking.VenueId &&
                booking.StartDate < b.EndDate &&
                booking.EndDate > b.StartDate);

            if (conflict)
            {
                ModelState.AddModelError("", "Venue already booked for this time.");
            }

            if (ModelState.IsValid)
            {
                booking.BookingDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }
    }
}