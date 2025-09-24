using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirlineBookingApp.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        public RegisterModel(IHttpClientFactory factory) => _factory = factory;

        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _factory.CreateClient("AirlineApi");
            var resp = await client.PostAsJsonAsync("Auth/register", new { Email, Password });
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Register failed");
                return Page();
            }

            // optionally auto-login after register:
            var loginResp = await client.PostAsJsonAsync("Auth/login", new { Email, Password });
            var json = await loginResp.Content.ReadFromJsonAsync<LoginModel.LoginResult>();
            if (json != null && !string.IsNullOrEmpty(json.token))
            {
                Response.Cookies.Append("X-Access-Token", json.token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Lax, Expires = DateTimeOffset.UtcNow.AddHours(6) });
                return RedirectToPage("/Flights");
            }

            return RedirectToPage("/Account/Login");
        }
}
    }
