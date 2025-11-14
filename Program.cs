using System;
using HotelManagementSystem.Services.Auth;    // Auth service
using HotelManagementSystem.Services.Hotel;   // <-- ADD this (HotelDataService & ReservationService)

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Caching (recommended for dashboard lists)
builder.Services.AddMemoryCache();

// Typed HttpClient for IAuthService (login)
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    var baseUrl = builder.Configuration["Apis:HotelBackend"];
    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("Missing configuration: Apis:HotelBackend");
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});

// Typed HttpClient for hotel data (rooms/reservations/users)
builder.Services.AddHttpClient<IHotelDataService, HotelDataService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Apis:HotelBackend"]!);
    client.Timeout = TimeSpan.FromSeconds(15);
});

// Typed HttpClient for creating reservations (create-reservation page)
builder.Services.AddHttpClient<IReservationService, ReservationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Apis:HotelBackend"]!);
    client.Timeout = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// .NET 8 static assets (keep your existing lines)
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
