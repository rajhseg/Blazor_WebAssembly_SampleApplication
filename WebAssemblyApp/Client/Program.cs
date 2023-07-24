using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebAssemblyApp.Client;
using WebAssemblyApp.Client.Models.Filters;
using WebAssemblyApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();
builder.Services.AddTransient<IJsConsole, JsConsole>();

builder.Services.AddScoped<TokenHandler>().AddHttpClient("api", client =>
     { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); }
).AddHttpMessageHandler<TokenHandler>();



await builder.Build().RunAsync();
