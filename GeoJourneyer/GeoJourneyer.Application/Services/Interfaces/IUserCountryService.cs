using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserCountryService
{
    IEnumerable<UserCountry> GetUserCountries(int userId);
    int AddUserCountry(UserCountry country);
    void UpdateUserCountry(UserCountry country);
}
