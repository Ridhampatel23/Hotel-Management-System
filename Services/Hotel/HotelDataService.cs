using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Rooms;
using HotelManagementSystem.Models.Reservations;
using HotelManagementSystem.Models.Users;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HotelManagementSystem.Services.Hotel
{
    public sealed class HotelDataService : IHotelDataService
    {
        private readonly HttpClient _http;
        private readonly ILogger<HotelDataService> _logger;
        private readonly IMemoryCache _cache;

        public HotelDataService(HttpClient http, ILogger<HotelDataService> logger, IMemoryCache cache)
        {
            _http = http;
            _logger = logger;
            _cache = cache;
        }

        public Task<IReadOnlyList<Room>> GetRoomsAsync(CancellationToken ct = default) =>
            GetOrSetAsync("dash.rooms", () => GetAsync<IReadOnlyList<Room>>("api/Rooms", ct), seconds: 30);

        public Task<IReadOnlyList<Reservation>> GetReservationsAsync(CancellationToken ct = default) =>
            GetOrSetAsync("dash.reservations", () => GetAsync<IReadOnlyList<Reservation>>("api/Reservations", ct), seconds: 30);

        public Task<IReadOnlyList<User>> GetUsersAsync(CancellationToken ct = default) =>
            GetOrSetAsync("dash.users", () => GetAsync<IReadOnlyList<User>>("api/Users", ct), seconds: 30);

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken ct)
        {
            try
            {
                using var resp = await _http.GetAsync(relativeUrl, ct);
                var body = await resp.Content.ReadAsStringAsync(ct);

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET {Url} failed: {Status} {Body}", relativeUrl, (int)resp.StatusCode, body);
                    return default;
                }

                return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                _logger.LogError(ex, "GET {Url} failed", relativeUrl);
                return default;
            }
        }

        private async Task<IReadOnlyList<T>> GetOrSetAsync<T>(string key, Func<Task<IReadOnlyList<T>?>> factory, int seconds)
        {
            if (_cache.TryGetValue(key, out IReadOnlyList<T>? cached) && cached is not null)
                return cached;

            var data = await factory() ?? Array.Empty<T>();
            _cache.Set(key, data, TimeSpan.FromSeconds(seconds));
            return data;
        }
    }
}
