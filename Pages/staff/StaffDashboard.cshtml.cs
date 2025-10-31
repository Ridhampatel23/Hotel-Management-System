using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelManagementSystem.Pages.staff
{
    public class StaffDashboardModel : PageModel
    {
        public string HotelName { get; set; } = "Grand Hotel";

        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int ReservedRooms { get; set; }
        public int MaintenanceRooms { get; set; }
        public double OccupancyRate { get; set; }

        public int ActiveReservations { get; set; }
        public int PendingReservations { get; set; }

        public decimal RevenueMTD { get; set; }

        public List<RecentActivity> RecentActivities { get; set; } = new();

        public class Room
        {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Status { get; set; }
        }

        public class Reservation
        {
            public int Id { get; set; }
            public string GuestName { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty; // Active, Pending, Completed
            public decimal Price { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class RecentActivity
        {
            public string GuestName { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
            public string TimeAgo { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://hotel-backend-o5hk.onrender.com");

            // 1️⃣ Fetch Rooms
            var roomsResponse = await client.GetAsync("/api/Rooms");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var roomsJson = await roomsResponse.Content.ReadAsStringAsync();
                var rooms = JsonConvert.DeserializeObject<List<Room>>(roomsJson) ?? new List<Room>();

                AvailableRooms = rooms.Count(r => r.Status == "Available");
                OccupiedRooms = rooms.Count(r => r.Status == "Occupied");
                ReservedRooms = rooms.Count(r => r.Status == "Reserved");
                MaintenanceRooms = rooms.Count(r => r.Status == "Maintenance");

                OccupancyRate = rooms.Any()
                    ? Math.Round((double)OccupiedRooms / rooms.Count * 100, 2)
                    : 0;
            }

            // 2️⃣ Fetch Reservations
            var reservationsResponse = await client.GetAsync("/api/Reservations");
            if (reservationsResponse.IsSuccessStatusCode)
            {
                var reservationsJson = await reservationsResponse.Content.ReadAsStringAsync();
                var reservations = JsonConvert.DeserializeObject<List<Reservation>>(reservationsJson) ?? new List<Reservation>();

                ActiveReservations = reservations.Count(r => r.Status == "Active");
                PendingReservations = reservations.Count(r => r.Status == "Pending");

                var firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                RevenueMTD = reservations
                    .Where(r => r.CreatedAt >= firstOfMonth)
                    .Sum(r => r.Price);

                RecentActivities = reservations
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .Select(r => new RecentActivity
                    {
                        GuestName = r.GuestName,
                        Action = r.Status switch
                        {
                            "Active" => $"Checked in - Room {r.Id}",
                            "Pending" => $"New booking - Room {r.Id}",
                            "Completed" => $"Checked out - Room {r.Id}",
                            _ => "Action unknown"
                        },
                        TimeAgo = GetTimeAgo(r.CreatedAt)
                    })
                    .ToList();
            }
        }

        private static string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.Now - dateTime;
            if (span.TotalDays >= 1)
                return $"{(int)span.TotalDays} days ago";
            if (span.TotalHours >= 1)
                return $"{(int)span.TotalHours} hours ago";
            if (span.TotalMinutes >= 1)
                return $"{(int)span.TotalMinutes} minutes ago";
            return "Just now";
        }
    }
}
