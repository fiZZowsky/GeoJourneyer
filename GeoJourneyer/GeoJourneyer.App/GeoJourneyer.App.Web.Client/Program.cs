using GeoJourneyer.App.Shared.Services;
using GeoJourneyer.App.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the GeoJourneyer.App.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<RegisterValidator>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
var proxyUrl = builder.Configuration["ProxyUrl"];

builder.Services
    .AddHttpClient<ApiProxyClient>(client =>
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        if (!string.IsNullOrEmpty(proxyUrl))
        {
            handler.UseProxy = true;
            handler.Proxy = new WebProxy(proxyUrl);
        }
        return handler;
    });
await builder.Build().RunAsync();
