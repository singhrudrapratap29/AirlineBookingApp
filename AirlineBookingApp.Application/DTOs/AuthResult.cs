using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Application.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Error { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
