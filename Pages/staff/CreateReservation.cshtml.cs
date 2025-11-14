using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using HotelManagementSystem.Services.Hotel;

// Type aliases to force the right DTOs
using RoomDto = HotelManagementSystem.Models.Rooms.Room;
using CreateResDto = HotelManagementSystem.Models.Reservations.CreateReservationDto;

namespace HotelManagementSystem.Pages.staff
{
    public class CreateReservationModel : PageModel
    {
        private readonly IHotelDataService _hotelData;
        private readonly IReservationService _reservations;

        public CreateReservationModel(IHotelDataService hotelData, IReservationService reservations)
        {
            _hotelData = hotelData;
            _reservations = reservations;
        }

        [BindProperty, Required] public string FullName { get; set; } = string.Empty;
        [BindProperty, EmailAddress] public string? Email { get; set; }
        [BindProperty] public string? PhoneNumber { get; set; }
        [BindProperty, Required] public string RoomType { get; set; } = string.Empty;
        [BindProperty, DataType(DataType.Date)] public DateTime CheckInDate { get; set; }
        [BindProperty, DataType(DataType.Date)] public DateTime CheckOutDate { get; set; }
        [BindProperty, Range(1, 20)] public int NumberOfGuests { get; set; } = 1;

        public string Message { get; private set; } = string.Empty;

        // IMPORTANT: use the DTO alias type here
        public List<RoomDto> AvailableRoomsList { get; private set; } = new();

        public async Task OnGetAsync(CancellationToken ct)
        {
            var allRooms = await _hotelData.GetRoomsAsync(ct);
            AvailableRoomsList = allRooms != null
                ? allRooms.Where(r => r.Status == "available").ToList()
                : new List<RoomDto>();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fix the validation errors.";
                await OnGetAsync(ct);
                return Page();
            }

            var dto = new CreateResDto
            {
                FullName = FullName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                RoomType = RoomType,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                NumberOfGuests = NumberOfGuests
            };

            var ok = await _reservations.CreateAsync(dto, ct);

            if (ok) return RedirectToPage("/staff/reservations");

            Message = "Failed to create reservation. Please try again.";
            await OnGetAsync(ct);
            return Page();
        }
    }
}
