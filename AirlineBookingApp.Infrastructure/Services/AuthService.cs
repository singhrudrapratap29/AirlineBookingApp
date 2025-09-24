using AirlineBookingApp.Application.DTOs;
using AirlineBookingApp.Application.Interfaces;
using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext ctx) => _context = ctx;

        public async Task<User?> RegisterAsync(string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email)) // user already available
                return null;
            var user = new User { Email = email, PasswordHash = Hash(password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        //LOgin
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            return Verify(password, user.PasswordHash) ? user : null;
        }
        public async Task<AuthResult> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return new AuthResult { Success = false, Error = "User not found" };

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var resetLink = $"https://localhost:7020/reset-password?token={user.ResetToken}";

            return new AuthResult { Success = true, Message = "Password reset link generated", Data = resetLink, Token = user.ResetToken };
        }

        public async Task<AuthResult> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                return new AuthResult { Success = false, Message = "Invalid or expired token" };

            user.PasswordHash = Hash(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            return new AuthResult { Success = true, Message = "Password reset successfully" };
        }

        // Password Hashing
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        private static bool Verify(string input, string hash)
        {
            return Hash(input) == hash;
        }

    }
}
