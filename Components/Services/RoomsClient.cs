using System.Net.Http.Json;

namespace MyBlazorApp.Components.Services;

public class RoomsClient
{
    private readonly HttpClient _http;

    public RoomsClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RoomDto>> GetAllRooms(CancellationToken ct = default)
    {
        // List endpoint
        var result = await _http.GetFromJsonAsync<List<RoomDto>>("/api/rooms", ct);
        return result ?? new();
    }

    public async Task<RoomDto?> GetRoomById(int id, CancellationToken ct = default)
    {
        // Details endpoint
        return await _http.GetFromJsonAsync<RoomDto>($"/api/Rooms/{id}", ct);
    }
}

public class RoomDto
{
    public int RoomId { get; set; }
    public string RoomType { get; set; } = "";
    public decimal Price { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}