using AirlineBookingApp.Domain.Entities;
using AirlineBookingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AirlineBookingApp.Web.Pages
{
    public class SeatsModel : PageModel
    {
        private readonly AppDbContext _context;
        public SeatsModel(AppDbContext context)
        {
            _context = context;
        }
        public Flight? Flight { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Flight = await _context.Flights
                .Include(f => f.Seats)
                .FirstOrDefaultAsync(f => f.Id == id);
            if(Flight == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
