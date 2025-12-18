namespace MyBlazorApp.Components.Models;

public class BookingRequest
{
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Guests { get; set; }
    public string? SpecialRequests { get; set; }
}