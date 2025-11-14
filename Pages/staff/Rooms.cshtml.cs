using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HotelManagementSystem.Pages.staff
{
    public class RoomsModel : PageModel
    {
        // Use the page-specific VM, not `Room`
        public List<StaffRoomVm> Rooms { get; set; } = new()
        {
            new StaffRoomVm { Id="1", Number="101", Type="Single", Price=100, Status="Available" },
            new StaffRoomVm { Id="2", Number="102", Type="Double", Price=150, Status="Occupied" },
            new StaffRoomVm { Id="3", Number="103", Type="Suite",  Price=250, Status="Available" },
        };

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }
        public void OnPost() { }
    }
}
