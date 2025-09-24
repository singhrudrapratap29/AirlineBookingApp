using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetBookingByEmailAsync(string email);
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<BookingDto> CreateBookingAsync(BookingRequestDto bookingRequestDto);
        Task<bool> CancelBookingAsync(CancelBookingRequestDto reuest);
    }
}
