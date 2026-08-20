using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AIInterviewCoach.UI;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using AIInterviewCoach.UI.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// Authentication Setup
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

// Configure HttpClient with JWT Handler
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();
builder.Services.AddScoped(sp => 
{
    var handler = sp.GetRequiredService<JwtAuthorizationMessageHandler>();
    // For WebAssembly, the inner handler needs to be WebAssemblyHttpMessageHandler
    // but HttpClientHandler is technically mapped to it by default.
    handler.InnerHandler = new HttpClientHandler(); 
    return new HttpClient(handler) { BaseAddress = new Uri("https://localhost:7229/api/") };
});

await builder.Build().RunAsync();
