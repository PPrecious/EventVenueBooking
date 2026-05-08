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

        // ✅ UPDATED INDEX WITH SEARCH + VIEW
        public async Task<IActionResult> Index(string search)
        {
            var bookings = _context.BookingDetailsView.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                bookings = bookings.Where(b =>
                    b.EventName.Contains(search) ||
                    b.BookingId.ToString().Contains(search));
            }

            return View(await bookings.ToListAsync());
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.Events = _context.Events.ToList();
            ViewBag.Venues = _context.Venues.ToList();
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            try
            {
                // ✅ Validate date order
                if (booking.EndDate <= booking.StartDate)
                {
                    ModelState.AddModelError("", "End date must be after start date.");
                }

                // ✅ Prevent double booking
                bool conflict = _context.Bookings.Any(b =>
                    b.VenueId == booking.VenueId &&
                    booking.StartDate < b.EndDate &&
                    booking.EndDate > b.StartDate);

                if (conflict)
                {
                    ModelState.AddModelError("", "Venue already booked for this time.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Events = _context.Events.ToList();
                    ViewBag.Venues = _context.Venues.ToList();
                    return View(booking);
                }

                booking.BookingDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error creating booking.";

                ViewBag.Events = _context.Events.ToList();
                ViewBag.Venues = _context.Venues.ToList();
                return View(booking);
            }
        }
    }
}