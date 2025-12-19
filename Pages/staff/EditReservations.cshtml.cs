using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Pages.staff
{
    public class EditReservationModel : PageModel
    {
        [BindProperty] public EditReservationVm Reservation { get; set; } = new();
        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) { Message = "Invalid reservation ID."; return; }

            var client = new HttpClient();
            var response = await client.GetAsync($"https://hotel-backend-o5hk.onrender.com/api/Reservation/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vm = JsonConvert.DeserializeObject<EditReservationVm>(json);
                Reservation = vm ?? new EditReservationVm { Id = id };
            }
            else
            {
                Message = "Failed to load reservation.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(Reservation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://hotel-backend-o5hk.onrender.com/api/Reservation/{Reservation.Id}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("/staff/reservations");

                Message = "Failed to update reservation.";
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }

            return Page();
        }
    }

    public class EditReservationVm
    {
        public string Id { get; set; } = string.Empty;

        [Required] public string FullName { get; set; } = string.Empty;
        [EmailAddress] public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required] public string RoomType { get; set; } = "Single";

        [DataType(DataType.Date)] public string CheckInDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        [DataType(DataType.Date)] public string CheckOutDate { get; set; } = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

        [Range(1, 20)] public int NumberOfGuests { get; set; } = 1;

        public string Status { get; set; } = "Pending";
    }
}
