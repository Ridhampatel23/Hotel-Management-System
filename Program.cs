using MyBlazorApp.Components;
using MyBlazorApp.Components.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// CHANGE THIS to your backend base URL
var apiBase = builder.Configuration["Api:BaseUrl"] ?? "https://YOUR-BACKEND-BASE-URL";

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// One CookieContainer per user circuit so the backend session cookie persists
builder.Services.AddScoped(_ => new CookieContainer());

builder.Services.AddScoped(sp =>
{
    var cookies = sp.GetRequiredService<CookieContainer>();
    var handler = new HttpClientHandler
    {
        CookieContainer = cookies,
        UseCookies = true,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    };
    return new HttpClient(handler) { BaseAddress = new Uri(apiBase) };
});

builder.Services.AddScoped<AuthClient>();
builder.Services.AddScoped<RoomsClient>();

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