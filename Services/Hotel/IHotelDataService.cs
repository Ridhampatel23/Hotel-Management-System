using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Rooms;
using HotelManagementSystem.Models.Reservations;
using HotelManagementSystem.Models.Users;

namespace HotelManagementSystem.Services.Hotel
{
    public interface IHotelDataService
    {
        Task<IReadOnlyList<Room>> GetRoomsAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Reservation>> GetReservationsAsync(CancellationToken ct = default);
        Task<IReadOnlyList<User>> GetUsersAsync(CancellationToken ct = default);
    }
}
