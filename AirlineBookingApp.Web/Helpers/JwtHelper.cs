using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AirlineBookingApp.Web.Helpers
{
    public class JwtHelper
    {
        public static string? GetEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(c=>c.Type == JwtRegisteredClaimNames.Email)?.Value;
        }
    }
}
