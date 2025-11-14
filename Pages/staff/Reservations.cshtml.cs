using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HotelManagementSystem.Pages.staff
{
    public class ReservationsModel : PageModel
    {
        public List<ReservationRowVm> Reservations { get; set; } = new()
        {
            new ReservationRowVm
            {
                FullName = "Alice Smith",
                Email = "alice@example.com",
                PhoneNumber = "555-0101",
                RoomType = "Single",
                CheckInDate = "2025-11-20",
                CheckOutDate = "2025-11-22",
                NumberOfGuests = 1,
                Status = "Confirmed"
            }
        };

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }
    }

    // View model tailored to the table columns
    public class ReservationRowVm
    {
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string CheckInDate { get; set; } = string.Empty;   // keep as string to match cshtml
        public string CheckOutDate { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
