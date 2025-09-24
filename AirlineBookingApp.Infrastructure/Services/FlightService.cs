using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Application.Interfaces;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Infrastructure.Services
{
    public class FlightService : IFlightService
    {
        private readonly AppDbContext _ctx;
        public FlightService(AppDbContext ctx) => _ctx = ctx;

        public async Task<List<FlightDto>> GetFlightsAsync(string? source = null, string? destination = null, DateTime? date = null)
        {
            var q = _ctx.Flights.AsQueryable();

            if (!string.IsNullOrWhiteSpace(source))
                q = q.Where(f => f.Source.ToLower().Contains(source.ToLower()));

            if (!string.IsNullOrWhiteSpace(destination))
                q = q.Where(f=>f.Destination.ToLower().Contains(destination.ToLower()));
            if (date.HasValue)
                q = q.Where(f => f.DepartureTime.Date == date.Value.Date);

            return await q.Select(f=> new FlightDto
                {
                    Id = f.Id,
                    FlightNumber = f.FlightNumber,
                    Source = f.Source,
                    Destination = f.Destination,
                    DepartureTime = f.DepartureTime,
                    ArrivalTime = f.ArrivalTime,
                    Seats = f.Seats.Select(s=> new SeatDto { 
                        Id = s.Id,
                        SeatNumber = s.SeatNumber,
                        IsBooked = s.IsBooked
                    }).ToList()
                }
                ).ToListAsync();

        }

        public async Task<FlightDto?> GetFlightWithSeatsAsync(int id)
        {

            return await _ctx.Flights
                .Where(f => f.Id == id)
                .Select(f=> new FlightDto { 
                    Id = f.Id,
                    FlightNumber = f.FlightNumber,
                    Source = f.Source,
                    Destination= f.Destination,
                    DepartureTime = f.DepartureTime,
                    ArrivalTime = f.ArrivalTime,
                    Seats = f.Seats.Select(
                            s=> new SeatDto { 
                                Id = s.Id,
                                SeatNumber = s.SeatNumber,
                                IsBooked = s.IsBooked
                            }
                        ).ToList()
                })
                .FirstOrDefaultAsync();
        }
        
    }
}
