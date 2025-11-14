using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.Auth
{
    public sealed class StaffLoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;
    }
}
