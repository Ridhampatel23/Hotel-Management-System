using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HotelManagementSystem.Pages.staff
{
    public class StaffLoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email and password are required.";
                return Page();
            }

            var client = new HttpClient();
            var payload = new
            {
                email = Email,
                password = Password
            };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://hotel-backend-o5hk.onrender.com/api/Auth/staff-login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent); // or use ILogger
            if (response.IsSuccessStatusCode)
            {
                // Login successful — redirect to staff dashboard (you can change this later)
                return RedirectToPage("/staff/Dashboard");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
