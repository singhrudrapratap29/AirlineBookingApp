using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using AirlineBookingApp.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AirlineBookingApp.Web.Pages
{
    public class MyBookingsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyBookingsModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Booking> Bookings { get; set; } = new();

        //Get Booking Details
        public async Task OnGet()
        {
            var context = _httpContextAccessor.HttpContext;
            var tokenExists = context?.Request.Cookies.ContainsKey("X-Access-Token") ?? false;
            string? userEmail = null;
            if (tokenExists)
            {
                var token = context?.Request.Cookies["X-Access-Token"];
                if (!string.IsNullOrEmpty(token))
                {
                    userEmail = JwtHelper.GetEmailFromToken(token);
                }
            }
            if (!string.IsNullOrEmpty(userEmail))
            {
                Bookings = await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Where(b => b.PassengerEmail == userEmail)
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
