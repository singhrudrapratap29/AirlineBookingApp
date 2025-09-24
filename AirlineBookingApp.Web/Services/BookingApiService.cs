using AirlineBookingApp.Application.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace AirlineBookingApp.Web.Services
{

    public class BookingApiService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient ClientWithToken()
        {
            var c = _factory.CreateClient("AirlineApi");
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["X-Access-Token"];
            if (!string.IsNullOrEmpty(token)) c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return c;
        }

        public async Task<BookingDto> CreateBookingAsync(BookingRequestDto request)
        {
            var client = ClientWithToken();
            var resp = await client.PostAsJsonAsync("bookings", request);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<BookingDto>() ?? throw new Exception("Empty response");
        }

        // other methods: GetByEmail, Cancel -> use ClientWithToken()
    }

}
