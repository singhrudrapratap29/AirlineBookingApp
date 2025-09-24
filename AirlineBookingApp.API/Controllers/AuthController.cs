using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Application.Interfaces;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AirlineBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        public AuthController(AuthService auth, IConfiguration config, ILogger<AuthController> logger)
        {
            _authService = auth;
            _config = config;
            _logger = logger;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
        {
            _logger.LogInformation("Register attempt for {Email}", req.Email);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _authService.RegisterAsync(req.Email, req.Password);
            if (user == null) return BadRequest(new { error = "User already exists" });

            _logger.LogInformation("User {Email} registered successfully", req.Email);
            return Ok(new { message = "Registered" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            _logger.LogInformation("Login attempt for {Email}", req.Email);
            var user = await _authService.LoginAsync(req.Email, req.Password);
            if (user == null) return Unauthorized(new { error = "Invalid credentials" });

            var token = GenerateToken(user);
            _logger.LogInformation("User {Email} logged in successfully", req.Email);
            return Ok(new { token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] FOrgotPasswordDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto.Email);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        private string GenerateToken(User user)
        {
            var key = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"]!;
            var audience = _config["Jwt:Audience"]!;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(6), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // DTO for requests
        public record RegisterRequestDto(
            [System.ComponentModel.DataAnnotations.EmailAddress] string Email,
            [System.ComponentModel.DataAnnotations.Required] string Password
        );

        public record LoginRequestDto(
            [System.ComponentModel.DataAnnotations.EmailAddress] string Email,
            [System.ComponentModel.DataAnnotations.Required] string Password
        );

    }
}
