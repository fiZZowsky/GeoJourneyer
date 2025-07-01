using GeoJourneyer.App.Shared.Services;
using GeoJourneyer.App.Web.Components;
using GeoJourneyer.App.Web.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the GeoJourneyer.App.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<RegisterValidator>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.Configuration["Api:BaseUrl"];
var proxyUrl = builder.Configuration["ProxyUrl"];

builder.Services
    .AddHttpClient<ApiProxyClient>(client =>
    {
        if (!string.IsNullOrEmpty(apiBaseUrl))
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(GeoJourneyer.App.Shared._Imports).Assembly,
        typeof(GeoJourneyer.App.Web.Client._Imports).Assembly);

app.Run();
