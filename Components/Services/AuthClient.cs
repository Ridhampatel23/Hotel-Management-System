using System.Net.Http.Json;
using System.Text.Json;
using MyBlazorApp.Components.Models;
using MyBlazorApp.Components.State;

namespace MyBlazorApp.Components.Services;

public class AuthClient
{
    private readonly HttpClient _http;
    private readonly SessionState _session;

    public AuthClient(HttpClient http, SessionState session)
    {
        _http = http;
        _session = session;
    }

    public async Task<bool> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-login", new LoginRequest(email, password), ct);
        if (!res.IsSuccessStatusCode) return false;

        // Be lenient with casing just in case
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var loginRes = await res.Content.ReadFromJsonAsync<LoginResponse>(opts, ct);
        if (loginRes is null) return false;

        _session.Set(new()
        {
            Id = loginRes.UserId.ToString(),                   // ← convert number → string for our app state
            Name = $"{loginRes.FirstName} {loginRes.LastName}".Trim(),
            Email = email,
            Role = loginRes.Role
        });

        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-signup", req, ct);
        return res.IsSuccessStatusCode;
    }
}