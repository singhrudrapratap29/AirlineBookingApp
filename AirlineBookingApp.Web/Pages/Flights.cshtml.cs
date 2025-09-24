using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using AirlineBookingApp.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AirlineBookingApp.Web.Pages
{
    public class FlightsModel : PageModel
    {
        private readonly FlightApiService _flightApiService;
        public FlightsModel(FlightApiService fapis)
        {
            _flightApiService = fapis;
        }
        public List<FlightDto> Flights { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? source { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? destination { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? date { get; set; }

        public async Task OnGetAsync(string? source, string? destination, DateTime? date)
        {
            Flights = await _flightApiService.GetFlightsAsync(source, destination, date);   

        }
        
    }
}
