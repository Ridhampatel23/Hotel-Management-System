using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Auth;
using HotelManagementSystem.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelManagementSystem.Pages.staff
{
    public class StaffLoginModel : PageModel
    {
        private readonly IAuthService _auth;
        private readonly ILogger<StaffLoginModel> _logger;

        public StaffLoginModel(IAuthService auth, ILogger<StaffLoginModel> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; private set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _auth.LoginAsync(
                new StaffLoginRequest { Email = Email, Password = Password }, ct);

            if (result.Success)
            {
                return RedirectToPage("/staff/StaffDashboard");
            }

            ErrorMessage = result.Error ?? "Login failed.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            _logger.LogInformation("Login failed for {Email}", Email);
            return Page();
        }
    }
}
