using BlazorApp21;
using BlazorApp21.Components;
using BlazorApp21.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddMudServices();

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<SupabaseService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
