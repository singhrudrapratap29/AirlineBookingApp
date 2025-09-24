using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;

namespace AirlineBookingApp.Web.Pages
{
    public class BookModel : PageModel
    {
        private AppDbContext _context;
        public BookModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Booking Booking { get; set; } = new();
        public int SeatId { get; set; }

        public void OnGet(int seatId)
        {
            SeatId = seatId;
        }
        public IActionResult OnPost(int seatId)
        {
            var seat = _context.Seats.FirstOrDefault(s=> s.Id == seatId);
            if (seat == null || seat.IsBooked)
            {
                ModelState.AddModelError("","Seat not available");
                return Page();
            }
            // Booking creation
            Booking.SeatId = seatId;
            Booking.FlightId = seat.FlightId;
            Booking.ReferenceNumber = Guid.NewGuid().ToString().Substring(0,8);
            Booking.BookingDate = DateTime.UtcNow;
            Booking.Status = "Confirmed";
            seat.IsBooked = true;

            _context.Bookings.Add(Booking);
            _context.SaveChanges();
            return RedirectToPage("/Flights");

        }
    }
}
