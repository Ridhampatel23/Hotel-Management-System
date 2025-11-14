namespace HotelManagementSystem.Models.Auth
{
    public sealed class AuthResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public string? Token { get; init; } // use if your backend returns a token
    }
}
