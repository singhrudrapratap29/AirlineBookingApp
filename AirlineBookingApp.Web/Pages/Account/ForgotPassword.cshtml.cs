using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace AirlineBookingApp.Web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ForgotPasswordModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? ResetLink { get; set; }
        public string? Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("AirlineApi");
            var response = await client.PostAsJsonAsync("auth/forgot-password", new { Email });

            if (response.IsSuccessStatusCode)
            {
                // For demo: API should return a token or reset link in the response
                var result = await response.Content.ReadFromJsonAsync<ForgotPasswordResult>();
                if (!string.IsNullOrEmpty(result?.Token))
                {
                    // Build the reset link using the token
                    var request = HttpContext.Request;
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    ResetLink = $"{baseUrl}/Account/ResetPassword?token={result.Token}";
                }
                else
                {
                    Message = "Unable to generate reset link. Please try again.";
                }
            }
            else
            {
                Message = "No account found with this email.";
            }

            return Page();
        }

        public class ForgotPasswordResult
        {
            public string? Token { get; set; }
        }
    }
}