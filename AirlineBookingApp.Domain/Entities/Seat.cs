using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
        //Foreign Key
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }
    }
}
