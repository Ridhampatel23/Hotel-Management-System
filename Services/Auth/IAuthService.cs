using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Auth;

namespace HotelManagementSystem.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(StaffLoginRequest request, CancellationToken ct = default);
    }
}
