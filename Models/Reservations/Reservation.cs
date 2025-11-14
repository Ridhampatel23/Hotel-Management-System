using System;

namespace HotelManagementSystem.Models.Reservations
{
    public sealed class Reservation
    {
        public int ReservationId { get; init; }
        public int UserId { get; init; }
        public int RoomId { get; init; }
        public DateTime CheckInDate { get; init; }    // OR string if your API returns strings
        public DateTime CheckOutDate { get; init; }   // OR string
        public decimal TotalAmount { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
