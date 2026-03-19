    using EventVenueBooking.Models;
    using Microsoft.EntityFrameworkCore;

    namespace EventVenueBooking.Data
{
    public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

        public DbSet<Venue> Venues { get; set; } = null!;

            public DbSet<Event> Events { get; set; } = null!;

        public DbSet<Booking> Bookings { get; set; } = null!;
    }
    }
