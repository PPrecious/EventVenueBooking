# EventVenueBooking

EventEase is a web-based Venue Booking System developed using ASP.NET Core MVC. 
The system is designed to help booking the organization EventEase efficiently manage venues, events, and bookings, eliminating manual errors such as double bookings and scheduling conflicts.

VenuesController.cs:
using EventVenueBooking.Data;
using EventVenueBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventVenueBooking.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();

            return View(venue);
        }

        // POST: Venues/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
            if (venue == null)
                return NotFound();

            return View(venue);
        }

        // POST: Venues/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Check existence
        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}

 EventsController.cs:
 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventVenueBooking.Data;
using EventVenueBooking.Models;

namespace EventVenueBooking.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

BookingsController.cs:
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

Venue.cs:
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

Event.cs:
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
        public DateTime EventDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [ForeignKey("Venue")]
        public int? VenueId { get; set; }

        public Venue? Venue { get; set; }

      public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

Booking.cs:
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVenueBooking.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; } = null!;

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; } = null!;
    }
}
