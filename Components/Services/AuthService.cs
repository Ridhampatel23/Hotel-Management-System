using System.Threading.Tasks;

namespace MyBlazorApp.Services
{
    public class AuthService
    {
        public bool IsLoggedIn { get; private set; } = false;
        public string Role { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;

        public Task LoginAsync(string role, string userName)
        {
            Role = role;
            UserName = userName;
            IsLoggedIn = true;
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            Role = string.Empty;
            UserName = string.Empty;
            IsLoggedIn = false;
            return Task.CompletedTask;
        }
    }
}
