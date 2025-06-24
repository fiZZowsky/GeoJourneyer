using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeoJourneyer.Infrastructure.Extensions;

public static class RepositoryCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IUserCountryRepository, UserCountryRepository>();
        services.AddScoped<ITravelPlanRepository, TravelPlanRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
