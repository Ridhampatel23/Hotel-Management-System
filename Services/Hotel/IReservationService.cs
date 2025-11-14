using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Reservations;

namespace HotelManagementSystem.Services.Hotel
{
    public interface IReservationService
    {
        Task<bool> CreateAsync(CreateReservationDto dto, CancellationToken ct = default);
    }
}
