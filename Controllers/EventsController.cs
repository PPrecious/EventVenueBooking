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

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(@event);

                _context.Add(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error creating event.";
                return View(@event);
            }
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

            try
            {
                if (!ModelState.IsValid)
                    return View(@event);

                _context.Update(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error updating event.";
                return View(@event);
            }
        }

        // ============================
        // UPDATED DELETE (YOUR REQUEST)
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var hasBookings = _context.Bookings.Any(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete event with active bookings.";
                return RedirectToAction("Index");
            }

            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction("Index");
            }

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var @event = await _context.Events.FindAsync(id);

                if (@event == null)
                {
                    TempData["Error"] = "Event not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error deleting event.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}