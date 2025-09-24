using AirlineBookingApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _svc;
        public FlightsController(IFlightService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? source, 
            [FromQuery] string? destination, [FromQuery] DateTime? date)
        {
            var flights = await _svc.GetFlightsAsync(source, destination, date);
            return Ok(flights);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var flight = await _svc.GetFlightWithSeatsAsync(id);
            if (flight == null) NotFound();
            return Ok(flight);
        }
    }
}
