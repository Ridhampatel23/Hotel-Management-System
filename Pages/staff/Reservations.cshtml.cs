using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace HotelManagementSystem.Pages.staff
{
    public class ReservationsModel : PageModel
    {
        public List<Reservation> Reservations { get; set; } = new();
        public string Message { get; set; } = string.Empty;

        // Fetch all reservations
        public async Task OnGetAsync()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("https://hotel-backend-o5hk.onrender.com/api/Reservation/all");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Reservations = JsonConvert.DeserializeObject<List<Reservation>>(json) ?? new();
                }
                else
                {
                    Message = "Failed to fetch reservations.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error fetching reservations: {ex.Message}";
            }
        }

        // Delete reservation
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Message"] = "Invalid reservation ID.";
                return RedirectToPage();
            }

            try
            {
                using var client = new HttpClient();
                var response = await client.DeleteAsync($"https://hotel-backend-o5hk.onrender.com/api/Reservation/{id}");

                TempData["Message"] = response.IsSuccessStatusCode
                    ? "Reservation deleted successfully."
                    : "Failed to delete reservation.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error deleting reservation: {ex.Message}";
            }

            return RedirectToPage(); // refreshes the list after delete
        }
    }

    // Reservation model (same as in EditReservation)
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
}
