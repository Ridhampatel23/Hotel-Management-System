using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Services.Hotel;
using RoomDto = HotelManagementSystem.Models.Rooms.Room;

namespace HotelManagementSystem.Pages.staff
{
    public class RoomsModel : PageModel
    {
        private readonly IHotelDataService _hotelData;
        private readonly ILogger<RoomsModel> _logger;

        public RoomsModel(IHotelDataService hotelData, ILogger<RoomsModel> logger)
        {
            _hotelData = hotelData;
            _logger = logger;
        }

        public List<RoomDto> Rooms { get; private set; } = new();

        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync(CancellationToken ct)
        {
            var apiRooms = await _hotelData.GetRoomsAsync(ct);

            if (apiRooms == null || !apiRooms.Any())
            {
                Message = "No rooms found.";
                return;
            }

            Rooms = apiRooms.ToList();

            _logger.LogInformation("Loaded {Count} rooms from API.", Rooms.Count);
        }

    }
}
