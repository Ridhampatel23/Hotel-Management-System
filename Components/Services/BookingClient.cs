using System.Net.Http.Json;
using MyBlazorApp.Components.Models;
using MyBlazorApp.Components.State;

namespace MyBlazorApp.Components.Services;

public class BookingClient
{
    private readonly HttpClient _http;
    private readonly CurrentUserState _userState;

    public BookingClient(HttpClient http, CurrentUserState userState)
    {
        _http = http;
        _userState = userState;
    }

    public async Task<bool> CreateBookingAsync(
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        int guests,
        string? specialRequests,
        CancellationToken ct = default)
    {
        if (!_userState.UserId.HasValue)
            throw new InvalidOperationException("User is not logged in.");

        var body = new BookingRequest
        {
            UserId = _userState.UserId.Value,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Guests = guests,
            SpecialRequests = specialRequests
        };

        // adjust the URL to whatever your backend uses
        var res = await _http.PostAsJsonAsync("/api/bookings", body, ct);
        return res.IsSuccessStatusCode;
    }
}