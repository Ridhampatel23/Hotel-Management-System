using System.Net.Http.Json;
using System.Text.Json;
using MyBlazorApp.Components.Models;

namespace MyBlazorApp.Components.Services;

public class AuthClient
{
    private readonly HttpClient _http;

    public AuthClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-login", new LoginRequest(email, password), ct);
        if (!res.IsSuccessStatusCode) return false;

        // We still parse the response to ensure it's valid JSON,
        // but we don't store anything in a session anymore.
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var loginRes = await res.Content.ReadFromJsonAsync<LoginResponse>(opts, ct);

        return loginRes is not null;
    }

    public async Task<bool> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-signup", req, ct);
        return res.IsSuccessStatusCode;
    }
}