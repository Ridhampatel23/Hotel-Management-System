using System;

namespace HotelManagementSystem.Models.Reservations
{
    public sealed class CreateReservationApiRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "created";
    }
}
