using System.Net.Http.Json;
using System.Text.Json;
using MyBlazorApp.Components.Models;
using MyBlazorApp.Components.State;

namespace MyBlazorApp.Components.Services;

public class AuthClient
{
    private readonly HttpClient _http;
    private readonly CurrentUserState _userState;

    public AuthClient(HttpClient http, CurrentUserState userState)
    {
        _http = http;
        _userState = userState;
    }

    public async Task<bool> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-login",
            new LoginRequest(email, password), ct);

        if (!res.IsSuccessStatusCode) return false;

        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var loginRes = await res.Content.ReadFromJsonAsync<LoginResponse>(opts, ct);
        if (loginRes is null) return false;

        // store minimal user info on client
        _userState.Set(
            loginRes.UserId,
            loginRes.FirstName,
            loginRes.LastName,
            loginRes.Role
        );

        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/api/auth/guest-signup", req, ct);
        return res.IsSuccessStatusCode;
    }

    // simple client-side logout: wipe the state
    public Task LogoutAsync()
    {
        _userState.Clear();
        return Task.CompletedTask;
    }
}