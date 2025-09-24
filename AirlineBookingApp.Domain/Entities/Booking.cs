using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AirlineBookingApp.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        // Foreign Keys
        public int FlightId {  get; set; }
        public Flight? Flight { get; set; }

        public int SeatId { get; set; }
        public Seat? Seat { get; set; }

        // Passenger details
        [Required(ErrorMessage ="Name is required")]
        public string PassengerName { get; set; } = string.Empty;
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email format")]
        public string PassengerEmail { get; set; } = string.Empty;

        // Metadata
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        //Coupon
        public string? CouponCode { get; set; }
        public decimal? DiscountApplied { get; set; }
    }
}
