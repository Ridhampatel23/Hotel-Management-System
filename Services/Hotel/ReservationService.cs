using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Reservations;
using Microsoft.Extensions.Logging;

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
                var resp = await _http.PostAsJsonAsync("api/Reservations", dto, ct);
                return resp.IsSuccessStatusCode;
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
    }
}
