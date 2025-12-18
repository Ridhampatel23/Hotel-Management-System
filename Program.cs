using MyBlazorApp.Components;
using MyBlazorApp.Components.Services;
using MyBlazorApp.Components.State;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Backend API base URL
var apiBase = builder.Configuration["Api:BaseUrl"] 
              ?? "https://YOUR-BACKEND-BASE-URL";

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Store logged-in user info on client
builder.Services.AddScoped<CurrentUserState>();
builder.Services.AddScoped<BookingState>();


// Basic HttpClient (no cookies needed)
builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    };

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(apiBase)
    };
});

// Services
builder.Services.AddScoped<AuthClient>();
builder.Services.AddScoped<RoomsClient>();
builder.Services.AddScoped<BookingClient>();   

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();