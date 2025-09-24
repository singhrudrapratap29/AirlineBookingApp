
using AirlineBookingApp.API.Middleware;
using AirlineBookingApp.Application.Interfaces;
using AirlineBookingApp.Infrastructure.Persistence;
using AirlineBookingApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var config = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options=> 
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));


// CORS
builder.Services.AddCors(options=>
    { 
        options.AddPolicy("AllowDev", p=> p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    }
);

//JWT settings
var jwtKey = config["Jwt:Key"] ?? "AirlineProjectIsAwesomeToShow!23456789";
var jwtIssuer = config["jwtIssuer"] ?? "AirlineBookingApp";
var audience = config["Jwt:Audience"] ?? "AirlineBookingUsers";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters { 
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateLifetime = true
    };
});
// Application services
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Logging 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();
// Add Global exception middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowDev");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
