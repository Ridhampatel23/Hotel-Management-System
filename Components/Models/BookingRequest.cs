namespace MyBlazorApp.Components.Models;
public class BookingRequest
{
    public int UserId { get; set; }
    public int RoomId { get; set; }

    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }

    public decimal TotalAmount { get; set; }
}
