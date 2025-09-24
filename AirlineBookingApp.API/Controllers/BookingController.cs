using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService svc)
        {
            _bookingService = svc;
        }
        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var bookings = await _bookingService.GetBookingByEmailAsync(email);
            return Ok(bookings);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequestDto request)
        {
            var booking = await _bookingService.CreateBookingAsync(request);
            return Ok(booking);
        }
        [HttpPut("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelBookingRequestDto request)
        {
            var success = await _bookingService.CancelBookingAsync(request);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
