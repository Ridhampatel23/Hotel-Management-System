using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Services.Hotel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// Aliases so we are 100% sure which types we mean
using RoomDto = HotelManagementSystem.Models.Rooms.Room;
using ResvDto = HotelManagementSystem.Models.Reservations.Reservation;
using UserDto = HotelManagementSystem.Models.Users.User;

namespace HotelManagementSystem.Pages.staff
{
    public class StaffDashboardModel : PageModel
    {
        private readonly IHotelDataService _svc;
        private readonly ILogger<StaffDashboardModel> _logger;

        public StaffDashboardModel(IHotelDataService svc, ILogger<StaffDashboardModel> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        public List<RoomDto> Rooms { get; private set; } = new();
        public List<ResvDto> Reservations { get; private set; } = new();
        public List<UserDto> Users { get; private set; } = new();

        public int AvailableRooms => Rooms.Count(r => r.Status == "available");
        public int OccupiedRooms  => Rooms.Count(r => r.Status == "occupied");
        public int TotalReservations => Reservations.Count;

        public string? ErrorMessage { get; private set; }

        public async Task OnGetAsync(CancellationToken ct)
        {
            var roomsTask = _svc.GetRoomsAsync(ct);
            var resTask   = _svc.GetReservationsAsync(ct);
            var usersTask = _svc.GetUsersAsync(ct);

            await Task.WhenAll(roomsTask, resTask, usersTask);

            var r1 = roomsTask.Result;
            var r2 = resTask.Result;
            var r3 = usersTask.Result;

            Rooms        = r1 != null ? r1.ToList() : new List<RoomDto>();
            Reservations = r2 != null ? r2.ToList() : new List<ResvDto>();
            Users        = r3 != null ? r3.ToList() : new List<UserDto>();

            if (Rooms.Count == 0 && Reservations.Count == 0 && Users.Count == 0)
            {
                ErrorMessage = "Couldn’t load data from the server. Please try again.";
                _logger.LogWarning("Dashboard loaded with empty datasets.");
            }

            _logger.LogInformation("Dashboard loaded: {Rooms} rooms, {Res} reservations, {Users} users",
                Rooms.Count, Reservations.Count, Users.Count);
        }
    }
}
