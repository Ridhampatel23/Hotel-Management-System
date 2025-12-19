using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Reservations;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HotelManagementSystem.Services.Hotel
{
    public sealed class ReservationService : IReservationService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(HttpClient http, ILogger<ReservationService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateReservationDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = 1;

                var apiBody = new CreateReservationApiRequest
                {
                    UserId = userId,
                    RoomId = dto.RoomId,
                    CheckInDate = dto.CheckInDate,
                    CheckOutDate = dto.CheckOutDate,
                    TotalAmount = 0m,
                    Status = "created"
                };

                var json = JsonSerializer.Serialize(apiBody);
                _logger.LogInformation("Sending reservation payload to API: {Payload}", json);

                var resp = await _http.PostAsJsonAsync("api/Reservations", apiBody, ct);

                _logger.LogInformation("Reservation API responded with status code: {StatusCode}", resp.StatusCode);

                if (!resp.IsSuccessStatusCode)
                {
                    var errorBody = await resp.Content.ReadAsStringAsync(ct);
                    _logger.LogError("Reservation API error response: {Body}", errorBody);
                    return false;
                }

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Create reservation failed (network)");
                return false;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Create reservation timed out");
                return false;
            }
        }

        public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Fetching reservations from API...");

                var reservations =
                    await _http.GetFromJsonAsync<List<Reservation>>("api/Reservations", ct);

                _logger.LogInformation("Fetched {Count} reservations from API.", reservations?.Count ?? 0);

                return reservations ?? new List<Reservation>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetAll reservations failed (network)");
                return new List<Reservation>();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "GetAll reservations timed out");
                return new List<Reservation>();
            }
        }
    }
}
