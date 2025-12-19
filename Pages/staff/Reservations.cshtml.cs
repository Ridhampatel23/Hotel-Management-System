using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Services.Hotel;
using HotelManagementSystem.Models.Reservations;
using RoomDto = HotelManagementSystem.Models.Rooms.Room;

namespace HotelManagementSystem.Pages.staff
{
    public class ReservationsModel : PageModel
    {
        private readonly IReservationService _reservations;
        private readonly IHotelDataService _hotelData;
        private readonly ILogger<ReservationsModel> _logger;

        public ReservationsModel(
            IReservationService reservations,
            IHotelDataService hotelData,
            ILogger<ReservationsModel> logger)
        {
            _reservations = reservations;
            _hotelData = hotelData;
            _logger = logger;
        }

        public List<ReservationRowVm> Reservations { get; private set; } = new();

        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync(CancellationToken ct)
        {
            var apiReservations = await _reservations.GetAllAsync(ct);

            var rooms = await _hotelData.GetRoomsAsync(ct) ?? new List<RoomDto>();
            var roomById = rooms.ToDictionary(r => r.RoomId, r => r);

            if (!apiReservations.Any())
            {
                Message = "No reservations found.";
                return;
            }

            Reservations = apiReservations
                .Select(r =>
                {
                    roomById.TryGetValue(r.RoomId, out var room);

                    return new ReservationRowVm
                    {
                        ReservationId = r.ReservationId,
                        UserId = r.UserId,
                        RoomId = r.RoomId,
                        RoomType = room?.RoomType ?? $"Room {r.RoomId}",
                        CheckInDate = r.CheckInDate,
                        CheckOutDate = r.CheckOutDate,
                        NumberOfGuests = 0, // backend doesn't return guests count
                        Status = string.IsNullOrWhiteSpace(r.Status) ? "N/A" : r.Status
                    };
                })
                .ToList();

            _logger.LogInformation("Mapped {Count} reservations for view.", Reservations.Count);
        }
    }

    public class ReservationRowVm
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }

        public string RoomType { get; set; } = string.Empty;

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
