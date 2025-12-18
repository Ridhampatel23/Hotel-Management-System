using System.Net.Http.Json;
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
        decimal totalAmount,
        CancellationToken ct = default)
    {
        if (!_userState.UserId.HasValue || _userState.UserId.Value <= 0)
            throw new InvalidOperationException("User is not logged in or user id is invalid.");

        if (roomId <= 0)
            throw new ArgumentException("Invalid roomId.");

        // Server requires check-in before check-out
        if (checkIn.Date >= checkOut.Date)
            throw new ArgumentException("checkIn must be before checkOut.");

        // IMPORTANT: camelCase names (typical ASP.NET Core JSON binding)
        var body = new
        {
            userId = _userState.UserId.Value,
            roomId = roomId,
            checkInDate = checkIn.Date,
            checkOutDate = checkOut.Date,
            totalAmount = totalAmount
        };

        var res = await _http.PostAsJsonAsync("/api/reservations", body, ct);

        if (!res.IsSuccessStatusCode)
        {
            var text = await res.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"Booking failed: {(int)res.StatusCode} {res.StatusCode}. Body: {text}");
            return false;
        }

        return true;
    }
}