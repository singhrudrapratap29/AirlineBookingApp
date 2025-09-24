using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Application.Interfaces;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Infrastructure.Services
{
    public class BookingService : IBookingService
    {

        private readonly AppDbContext _ctx;
        public BookingService(AppDbContext ctx) => _ctx = ctx;

        public async Task<BookingDto> CreateBookingAsync(BookingRequestDto bookingRequestDto)
        {
            var flight = await _ctx.Flights.FindAsync(bookingRequestDto.FlightId);
            var seat = await _ctx.Seats.FindAsync(bookingRequestDto.SeatId);

            if (flight == null || seat == null || seat.IsBooked)
                throw new InvalidOperationException("Invalid flight/seat selection");

            seat.IsBooked = true;

            var booking = new Booking
            {
                ReferenceNumber = $"BK-{Guid.NewGuid().ToString().Substring(0,8)}",
                SeatId = bookingRequestDto.SeatId,
                FlightId = bookingRequestDto.FlightId,
                PassengerName = bookingRequestDto.PassengerName,
                PassengerEmail = bookingRequestDto.PassengerEmail,
                BookingDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            await _ctx.Bookings.AddAsync(booking);
            await _ctx.SaveChangesAsync();

            return new BookingDto { 
                Id = booking.Id,
                ReferenceNumber = booking.ReferenceNumber,
                PassengerName = booking.PassengerName,
                PassengerEmail = booking.PassengerEmail,
                Status = booking.Status,
                FlightNumber = flight.FlightNumber,
                Source = flight.Source,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,
                SeatNumber = seat.SeatNumber
            };

        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            return await _ctx.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .Where(b => b.Id == id)
                .Select(b => new BookingDto {
                    Id = b.Id,
                    ReferenceNumber = b.ReferenceNumber,
                    PassengerName = b.PassengerName,
                    PassengerEmail = b.PassengerEmail,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    FlightNumber = b.Flight.FlightNumber,
                    Source = b.Flight.Source,
                    Destination = b.Flight.Destination,
                    DepartureTime = b.Flight.DepartureTime,
                    SeatNumber = b.Seat.SeatNumber
                })
                .FirstOrDefaultAsync();
        }
        

        public async Task<List<BookingDto>> GetBookingByEmailAsync(string email)
        {
            return await _ctx.Bookings
                .Where(b=> b.PassengerEmail == email)
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .Select(b=> new BookingDto { 
                    Id = b.Id,
                    ReferenceNumber = b.ReferenceNumber,
                    PassengerEmail = b.PassengerEmail,
                    PassengerName = b.PassengerName,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    FlightNumber = b.Flight.FlightNumber,
                    Source = b.Flight.Source,
                    Destination = b.Flight.Destination,
                    DepartureTime = b.Flight.DepartureTime,
                    SeatNumber = b.Seat.SeatNumber
                })
                .ToListAsync();
        }
        public async Task<bool> CancelBookingAsync(CancelBookingRequestDto request)
        {
            var booking = await _ctx.Bookings
                .Include(b => b.Seat).FirstOrDefaultAsync(b =>b.Id == request.BookingId);

            if (booking == null) return false;

            booking.Status = "Cancelled";
            if (booking.Seat != null) booking.Seat.IsBooked = false;

            await _ctx.SaveChangesAsync();
            return true;
        }

        

        
    }
}
