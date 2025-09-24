using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Domain.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        //Navigation property
        public List<Seat> Seats { get; set; } = new();
    }
}
