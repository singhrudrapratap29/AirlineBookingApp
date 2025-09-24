using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Application.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
    }
}
