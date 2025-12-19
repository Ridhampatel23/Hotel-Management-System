using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using HotelManagementSystem.Services.Hotel;

using RoomDto = HotelManagementSystem.Models.Rooms.Room;
using CreateResDto = HotelManagementSystem.Models.Reservations.CreateReservationDto;

namespace HotelManagementSystem.Pages.staff
{
    public class CreateReservationModel : PageModel
    {
        private readonly IHotelDataService _hotelData;
        private readonly IReservationService _reservations;
        private readonly ILogger<CreateReservationModel> _logger;

        public CreateReservationModel(
            IHotelDataService hotelData,
            IReservationService reservations,
            ILogger<CreateReservationModel> logger)
        {
            _hotelData = hotelData;
            _reservations = reservations;
            _logger = logger;
        }

        [BindProperty, Required] public string FullName { get; set; } = string.Empty;
        [BindProperty, EmailAddress] public string? Email { get; set; }
        [BindProperty] public string? PhoneNumber { get; set; }


        [BindProperty, DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [BindProperty, Range(1, 20)]
        public int NumberOfGuests { get; set; } = 1;

        [BindProperty, Required]
        public int RoomId { get; set; }

        public string Message { get; private set; } = string.Empty;

        public List<RoomDto> AvailableRoomsList { get; private set; } = new();

        public async Task OnGetAsync(CancellationToken ct)
        {
            if (CheckInDate == default)  CheckInDate = DateTime.Today;
            if (CheckOutDate == default) CheckOutDate = DateTime.Today.AddDays(1);

            var allRooms = await _hotelData.GetRoomsAsync(ct);
            AvailableRoomsList = allRooms != null
                ? allRooms.Where(r => r.Status == "available").ToList()
                : new List<RoomDto>();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var err in kvp.Value.Errors)
                    {
                        _logger.LogError("Model error for {Key}: {Error}", kvp.Key, err.ErrorMessage);
                    }
                }

                Message = "Please fix the validation errors.";
                await OnGetAsync(ct);
                return Page();
            }

            // find room to optionally fill RoomType (for DTO only)
            var allRooms = await _hotelData.GetRoomsAsync(ct);
            var room = allRooms?.FirstOrDefault(r => r.RoomId == RoomId);

            var dto = new CreateResDto
            {
                FullName = FullName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                RoomType = room?.RoomType ?? string.Empty,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                NumberOfGuests = NumberOfGuests,
                RoomId = RoomId
            };

            _logger.LogInformation(
                "Creating reservation for {Name}, room {RoomId}, {CheckIn} - {CheckOut}",
                dto.FullName, dto.RoomId, dto.CheckInDate, dto.CheckOutDate);

            var ok = await _reservations.CreateAsync(dto, ct);

            if (ok)
            {
                return RedirectToPage("/staff/reservations");
            }

            Message = "Failed to create reservation. Please try again.";
            await OnGetAsync(ct);
            return Page();
        }
    }
}
