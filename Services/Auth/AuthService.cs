using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Auth;
using Microsoft.Extensions.Logging;

namespace HotelManagementSystem.Services.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient http, ILogger<AuthService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<AuthResult> LoginAsync(StaffLoginRequest request, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http.PostAsJsonAsync("api/Auth/staff-login", request, ct);
                var content = await resp.Content.ReadAsStringAsync(ct);

                if (resp.IsSuccessStatusCode)
                {
                    return new AuthResult { Success = true };
                }

                _logger.LogWarning("Staff login failed. Status: {Status}. Body: {Body}", (int)resp.StatusCode, content);
                return new AuthResult { Success = false, Error = "Invalid email or password." };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during staff login");
                return new AuthResult { Success = false, Error = "Network error. Please try again." };
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Login request timed out");
                return new AuthResult { Success = false, Error = "Request timed out. Please try again." };
            }
        }
    }
}
