using AirlineBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirlineBookingApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // make FlightNumber unique
            modelBuilder.Entity<Flight>()
                .HasIndex(f => f.FlightNumber)
                .IsUnique();

            // Relationships
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b=>b.Seat)
                .WithMany()
                .HasForeignKey(b=>b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
