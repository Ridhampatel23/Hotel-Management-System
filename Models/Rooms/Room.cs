namespace HotelManagementSystem.Models.Rooms
{
    public sealed class Room
    {
        public int RoomId { get; init; }
        public string RoomType { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
