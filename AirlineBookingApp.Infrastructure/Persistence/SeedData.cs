using AirlineBookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Infrastructure.Persistence
{
    public class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Flights.Any()) // skip if any flight data available
                return;

            //sample flights
            var flights = new List<Flight> { 
                new Flight {
                    FlightNumber = "AI101",
                    Source = "Delhi",
                    Destination = "Mumbai",
                    DepartureTime = DateTime.UtcNow.AddHours(5),
                    ArrivalTime = DateTime.UtcNow.AddHours(7),
                    Price = 5000,
                    Seats = GenerateSeats(20)
                },
                new Flight {
                    FlightNumber = "AI202",
                    Source = "Delhi",
                    Destination = "Banglore",
                    DepartureTime = DateTime.UtcNow.AddHours(8),
                    ArrivalTime = DateTime.UtcNow.AddHours(11),
                    Price = 7000,
                    Seats = GenerateSeats(20)
                }
            };
            context.Flights.AddRange(flights);
            context.SaveChanges();
        }
        private static List<Seat> GenerateSeats(int count)
        {
            var seats = new List<Seat>();
            for(int i=1; i<=count; i++)
            {
                seats.Add(new Seat { 
                    SeatNumber = $"S{i:D2}",
                    IsBooked = false
                });
            }
            return seats;
        }
    }
}
