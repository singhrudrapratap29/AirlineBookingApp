using System.Net.Http;
using System.Net.Http.Json;
using AirlineBookingApp.Application.DTOs;

namespace AirlineBookingApp.Web.Services
{
    public class FlightApiService
    {
        private readonly HttpClient _http;
        public FlightApiService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("AirlineApi");
        }

        public async Task<List<FlightDto>> GetFlightsAsync(
            string? source=null, string? destination=null, DateTime? date = null)
        {
            var query = new List<string>();
            if (!string.IsNullOrEmpty(source)) query.Add($"source={source}");
            if (!string.IsNullOrEmpty(destination)) query.Add($"destination={destination}");

            var url = "flights";
            if (query.Count > 0) url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<List<FlightDto>>(url) ?? new List<FlightDto>();
        }

        public async Task<FlightDto?> GetFlightWithSeatAsync(int id)
        {
            return await _http.GetFromJsonAsync<FlightDto>($"flights/{id}");
        }
    }
}
