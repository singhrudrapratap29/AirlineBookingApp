using AirlineBookingApp.Infrastructure.Persistence;
using AirlineBookingApp.Web.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("AirlineApi", client=> {
    client.BaseAddress = new Uri("https://localhost:7020/api/");
});
builder.Services.AddScoped<FlightApiService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Deafult route
app.MapGet("/", context => {
    context.Response.Redirect("/Flights");
    return Task.CompletedTask;
});
app.MapRazorPages();

app.Run();
