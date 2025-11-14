using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Pages.staff
{
    public class AddEditRoomModel : PageModel
    {
        [BindProperty] public StaffRoomVm Room { get; set; } = new();
        public bool IsEdit => !string.IsNullOrWhiteSpace(Room.Id);
        public string Message { get; set; } = string.Empty;

        public void OnGet(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                // TODO: Load room by id from backend/service
                Room = new StaffRoomVm
                {
                    Id = id,
                    Number = "101",
                    Type = "Single",
                    Price = 120,
                    Status = "Available"
                };
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fix validation errors.";
                return Page();
            }

            // TODO: Save to backend/service
            Message = IsEdit ? "Room updated." : "Room added.";
            return RedirectToPage("/staff/Rooms");
        }
    }

    // Page-specific ViewModel (does NOT collide with Models.Rooms.Room)
    public class StaffRoomVm
    {
        public string Id { get; set; } = string.Empty;

        [Required] public string Number { get; set; } = string.Empty;
        [Required] public string Type { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required] public string Status { get; set; } = "Available";
    }
}
