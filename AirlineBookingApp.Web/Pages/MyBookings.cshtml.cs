using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AirlineBookingApp.Web.Pages
{
    public class MyBookingsModel : PageModel
    {
        private readonly AppDbContext _context;
        public MyBookingsModel(AppDbContext context)
        {
            _context    = context;

        }
        public List<Booking> Bookings { get; set; } = new();
        //Get Booking Details
        public async Task OnGet(string? email)
        {
            if(!string.IsNullOrEmpty(email))
            {
                Bookings = await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Where(b => b.PassengerEmail == email)
                    .ToListAsync();
            }
        }
        // Cancel Booking Details
        public async Task<IActionResult> OnPostCancelAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
            if(booking != null)
            {
                booking.Status = "Cancelled";
                booking.Seat.IsBooked = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/MyBookings", new { email=booking?.PassengerEmail });
        }
    }
}
