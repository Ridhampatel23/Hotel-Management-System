namespace MyBlazorApp.Components.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    public ApiClient(HttpClient http) => _http = http;

    // Example method to get sample dashboard data
    public async Task<int> GetActiveBookingsCountAsync(CancellationToken ct = default)
    {
        var res = await _http.GetAsync("/dashboard/active-bookings-count", ct);
        if (!res.IsSuccessStatusCode) return 0;

        var text = await res.Content.ReadAsStringAsync(ct);
        return int.TryParse(text, out var n) ? n : 0;
    }
}