namespace MyBlazorApp.Components.State;

public class BookingState
{
    // A simple booking record to store in-memory
    public class BookingItem
    {
        public int RoomId { get; init; }
        public string RoomType { get; init; } = "";
        public decimal PricePerNight { get; init; }
        public DateTime CheckIn { get; init; }
        public DateTime CheckOut { get; init; }

        // Optional fields (keep if your UI still collects them)
        public int? Guests { get; init; }
        public string? FullName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }

        public decimal TotalAmount { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public int Nights => Math.Max(1, (CheckOut.Date - CheckIn.Date).Days);
    }

    private readonly List<BookingItem> _bookings = new();

    public IReadOnlyList<BookingItem> Bookings => _bookings;
    public bool HasAnyBookings => _bookings.Count > 0;

    // Add a booking (this replaces your old Set method conceptually)
    public void Add(BookingItem item)
    {
        _bookings.Insert(0, item); // newest first
    }

    // For convenience if you want to clear everything
    public void ClearAll()
    {
        _bookings.Clear();
    }

    // Optional: remove by index (simple demo)
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _bookings.Count) return;
        _bookings.RemoveAt(index);
    }
}