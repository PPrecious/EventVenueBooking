using EventVenueBooking.Data;
using EventVenueBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;

namespace EventVenueBooking.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public VenuesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public async Task<IActionResult> Create(Venue venue, IFormFile imageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(venue);

                // Upload Image to Azure Blob Storage
                if (imageFile != null && imageFile.Length > 0)
                {
                    var container = new BlobContainerClient(
                        _configuration["AzureBlobStorage"],
                        "venue-images"
                    );

                    await container.CreateIfNotExistsAsync();

                    var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    var blob = container.GetBlobClient(fileName);

                    using var stream = imageFile.OpenReadStream();
                    await blob.UploadAsync(stream, true);

                    venue.ImageUrl = blob.Uri.ToString();
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error creating venue.";
                return View(venue);
            }
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
        public async Task<IActionResult> Edit(int id, Venue venue, IFormFile imageFile)
        {
            if (id != venue.VenueId)
                return NotFound();

            try
            {
                if (!ModelState.IsValid)
                    return View(venue);

                // Update Image if new file uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var container = new BlobContainerClient(
                        _configuration["AzureBlobStorage"],
                        "venue-images"
                    );

                    await container.CreateIfNotExistsAsync();

                    var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    var blob = container.GetBlobClient(fileName);

                    using var stream = imageFile.OpenReadStream();
                    await blob.UploadAsync(stream, true);

                    venue.ImageUrl = blob.Uri.ToString();
                }

                _context.Update(venue);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(venue.VenueId))
                    return NotFound();
                else
                    throw;
            }
            catch
            {
                TempData["Error"] = "Error updating venue.";
                return View(venue);
            }
        }

        // GET: Venues/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            // ✅ PREVENT DELETE (EARLY CHECK)
            var hasBookings = _context.Bookings.Any(b => b.VenueId == id);
            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete venue with active bookings.";
                return RedirectToAction(nameof(Index));
            }

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
            try
            {
                // ✅ FINAL PROTECTION
                var hasBookings = _context.Bookings.Any(b => b.VenueId == id);

                if (hasBookings)
                {
                    TempData["Error"] = "Cannot delete venue with active bookings.";
                    return RedirectToAction(nameof(Index));
                }

                var venue = await _context.Venues.FindAsync(id);

                if (venue != null)
                {
                    _context.Venues.Remove(venue);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error deleting venue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Check existence
        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}