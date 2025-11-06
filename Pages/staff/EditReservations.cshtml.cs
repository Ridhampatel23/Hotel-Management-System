using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

public class Reservation
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
}

namespace HotelManagementSystem.Pages.staff
{
    public class EditReservationModel : PageModel
    {
        [BindProperty]
        public Reservation Reservation { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) { Message = "Invalid reservation ID."; return; }

            var client = new HttpClient();
            var response = await client.GetAsync($"https://hotel-backend-o5hk.onrender.com/api/Reservation/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Reservation = JsonConvert.DeserializeObject<Reservation>(json) ?? new();
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
                {
                    Message = "Reservation updated successfully.";
                    return RedirectToPage("/staff/reservations");
                }
                else
                {
                    Message = "Failed to update reservation.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }

            return Page();
        }
    }
}
