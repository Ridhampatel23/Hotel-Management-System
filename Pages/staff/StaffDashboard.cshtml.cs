using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelManagementSystem.Pages.staff
{
    public class StaffDashboardModel : PageModel
    {
        private readonly HttpClient _client;

        public StaffDashboardModel()
        {
            _client = new HttpClient();
        }

        // Data
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<User> Users { get; set; } = new List<User>();

        // Stats
        public int AvailableRooms => Rooms.FindAll(r => r.Status == "available").Count;
        public int OccupiedRooms => Rooms.FindAll(r => r.Status == "occupied").Count;
        public int TotalReservations => Reservations.Count;
        public int Reserved => Reservations.FindAll(r => r.Status == "reserved").Count;
        public int CheckedIn => Reservations.FindAll(r => r.Status == "checkedin").Count;
        public int CheckedOut => Reservations.FindAll(r => r.Status == "checkedout").Count;
        public int Cancelled => Reservations.FindAll(r => r.Status == "cancelled").Count;

        public async Task OnGetAsync()
        {
            await LoadRooms();
            await LoadReservations();
            await LoadUsers();
        }

        private async Task LoadRooms()
        {
            var response = await _client.GetAsync("https://hotel-backend-o5hk.onrender.com/api/Rooms");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Rooms = JsonConvert.DeserializeObject<List<Room>>(json);
            }
        }

        private async Task LoadReservations()
        {
            var response = await _client.GetAsync("https://hotel-backend-o5hk.onrender.com/api/Reservations");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Reservations = JsonConvert.DeserializeObject<List<Reservation>>(json);
            }
        }

        private async Task LoadUsers()
        {
            var response = await _client.GetAsync("https://hotel-backend-o5hk.onrender.com/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Users = JsonConvert.DeserializeObject<List<User>>(json);
            }
        }

        // Models
        public class Room
        {
            public int RoomId { get; set; }
            public string RoomType { get; set; }
            public int Price { get; set; }
            public string Status { get; set; }
        }

        public class Reservation
        {
            public int ReservationId { get; set; }
            public int UserId { get; set; }
            public int RoomId { get; set; }
            public string CheckInDate { get; set; }
            public string CheckOutDate { get; set; }
            public int TotalAmount { get; set; }
            public string Status { get; set; }

            // Placeholder for future enhancement
            public string UserName { get; set; } = "Unknown"; 
            public string RoomType { get; set; } = "Unknown"; 
        }

        public class User
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }
    }
}
