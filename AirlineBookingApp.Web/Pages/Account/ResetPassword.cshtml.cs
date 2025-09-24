using AirlineBookingApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace AirlineBookingApp.Web.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ResetPasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AirlineApi");
        }

        [BindProperty]
        public string Token { get; set; } = string.Empty;

        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;

        public string? Message { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet(string token)
        {
            Token = token; // capture token from query string
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var payload = new { Token, NewPassword };

            var resp = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", payload);
            if (resp.IsSuccessStatusCode)
            {
                var result = await resp.Content.ReadFromJsonAsync<AuthResult>();
                Message = result?.Message ?? "Password reset successful!";
                IsSuccess = true;
            }
            else
            {
                var error = await resp.Content.ReadAsStringAsync();
                Message = $"Failed: {error}";
                IsSuccess = false;
            }

            return Page();
        }
    }

    //public class AuthResult
    //{
    //    public bool Success { get; set; }
    //    public string Message { get; set; } = string.Empty;
    //}
}
