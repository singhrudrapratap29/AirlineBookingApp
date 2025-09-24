using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace AirlineBookingApp.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        public LoginModel(IHttpClientFactory factory) => _factory = factory;

        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _factory.CreateClient("AirlineApi");
            var resp = await client.PostAsJsonAsync("auth/login", new { Email, Password });
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }

            var json = await resp.Content.ReadFromJsonAsync<LoginResult>();
            if (json == null || string.IsNullOrEmpty(json.token)) { ModelState.AddModelError("", "Login failed"); return Page(); }

            Response.Cookies.Append("X-Access-Token", json.token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(6)
            });

            return RedirectToPage("/Flights");
        }

        public class LoginResult { public string token { get; set; } = ""; }
    }
}
