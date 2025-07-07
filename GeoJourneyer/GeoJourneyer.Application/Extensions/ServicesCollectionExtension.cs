using GeoJourneyer.Application.Services;
using GeoJourneyer.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GeoJourneyer.Application.Extensions;

public static class ServicesCollectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IUserCountryService, UserCountryService>();
        services.AddScoped<ITravelPlanService, TravelPlanService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFriendRequestService, FriendRequestService>();
        services.AddSingleton<ITokenService, TokenService>();
    }
}
