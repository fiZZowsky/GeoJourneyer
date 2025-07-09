using GeoJourneyer.Domain;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserCountryService
{
    IEnumerable<UserCountry> GetUserCountries(int userId);
    int AddUserCountry(UserCountry country);
    void UpdateUserCountry(UserCountry country);
    bool DeleteUserCountry(int id);
}
