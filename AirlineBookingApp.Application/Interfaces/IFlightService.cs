using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Domain.Entities;

namespace AirlineBookingApp.Application.Interfaces
{
    public interface IFlightService
    {
        Task<List<FlightDto>> GetFlightsAsync(string? source=null, string? destination = null, DateTime? date=null);
        Task<FlightDto?> GetFlightWithSeatsAsync(int id);
    }
}
