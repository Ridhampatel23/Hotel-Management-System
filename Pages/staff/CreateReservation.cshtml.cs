using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System;

namespace HotelManagementSystem.Pages.staff
{
    public class CreateReservationModel : PageModel
    {
        [BindProperty] public string FullName { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string PhoneNumber { get; set; } = string.Empty;
        [BindProperty] public string RoomType { get; set; } = string.Empty;
        [BindProperty] public DateTime CheckInDate { get; set; }
        [BindProperty] public DateTime CheckOutDate { get; set; }
        [BindProperty] public int NumberOfGuests { get; set; }

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(RoomType))
            {
                Message = "Please fill in all required fields.";
                return Page();
            }

            var client = new HttpClient();

            var reservationData = new
            {
                fullName = FullName,
                email = Email,
                phoneNumber = PhoneNumber,
                roomType = RoomType,
                checkInDate = CheckInDate.ToString("yyyy-MM-dd"),
                checkOutDate = CheckOutDate.ToString("yyyy-MM-dd"),
                numberOfGuests = NumberOfGuests
            };

            var json = JsonConvert.SerializeObject(reservationData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 🔗 Replace this URL with your actual backend endpoint for reservations
            var response = await client.PostAsync("https://hotel-backend-o5hk.onrender.com/api/Reservation/create", content);

            if (response.IsSuccessStatusCode)
            {
                Message = "Reservation created successfully!";
                ModelState.Clear();
            }
            else
            {
                Message = "Failed to create reservation. Please try again.";
            }

            return Page();
        }
    }
}
