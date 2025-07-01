using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using GeoJourneyer.App.Services;
using GeoJourneyer.App.Shared.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;

namespace GeoJourneyer.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the GeoJourneyer.App.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddScoped<AuthState>();
            builder.Services.AddScoped<RegisterValidator>();
            builder.Services
                .AddBlazorise()
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

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

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
