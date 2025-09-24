using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using AirlineBookingApp.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
        [BindProperty]
        public string? CouponCode { get; set; }
        public int SeatId { get; set; }
        public string? UserEmail { get; set; }

        public decimal BookingTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAfterDiscount { get; set; }

        public void OnGet(int seatId)
        {
            var token = HttpContext.Request.Cookies["X-Access-Token"];
            if (!string.IsNullOrEmpty(token))
            {
                UserEmail = JwtHelper.GetEmailFromToken(token);
            }
            SeatId = seatId;
            Booking.PassengerEmail = UserEmail;
            Booking.PassengerName = UserEmail;
        }
        public async Task<IActionResult> OnPostAsync(int seatId)
        {
            var seat = _context.Seats
                .Include(s=>s.Flight)
                .FirstOrDefault(s=> s.Id == seatId);
            if (seat == null || seat.IsBooked)
            {
                ModelState.AddModelError("","Seat not available");
                return Page();
            }
            BookingTotal = seat.Flight.Price;

            // Coupon logic
            if (!string.IsNullOrEmpty(CouponCode))
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.Code == CouponCode && c.IsActive && c.ExpiryDate > DateTime.UtcNow);
                if(coupon != null)
                {
                    Discount = coupon.DiscountAmount > 0
                       ? coupon.DiscountAmount
                       : (coupon.DiscountPercent.HasValue ? (BookingTotal * coupon.DiscountPercent.Value / 100) : 0);

                    TotalAfterDiscount = BookingTotal - Discount;
                }
                else
                {
                    Discount = 0;
                    TotalAfterDiscount = BookingTotal;
                    ModelState.AddModelError("CouponCode", "Invalid or expired coupon.");
                }
            }
            else
            {
                Discount = 0;
                TotalAfterDiscount = BookingTotal;
                ModelState.AddModelError("CouponCode", "Invalid or expired coupon.");
            }
            var token = HttpContext.Request.Cookies["X-Access-Token"];
            if (!string.IsNullOrEmpty(token))
            {
                UserEmail = JwtHelper.GetEmailFromToken(token);
            }
            if (!ModelState.IsValid)
            {
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
            return RedirectToPage("/MyBookings");

        }
    }
}
