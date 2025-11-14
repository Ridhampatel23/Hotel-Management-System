using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.Reservations
{
    public sealed class CreateReservationDto
    {
        [Required] public string FullName { get; init; } = string.Empty;
        [EmailAddress] public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        [Required] public string RoomType { get; init; } = string.Empty;
        [Required] public DateTime CheckInDate { get; init; }
        [Required] public DateTime CheckOutDate { get; init; }
        [Range(1, 20)] public int NumberOfGuests { get; init; }
    }
}
