using GeoJourneyer.Application.Services.Interfaces;

namespace GeoJourneyer.Application.Services;

public class UserCountryService : IUserCountryService
{
    private readonly IUserCountryRepository _repository;

    public UserCountryService(IUserCountryRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<UserCountry> GetUserCountries(int userId)
        => _repository.GetAll(new UserCountryQuery { UserId = userId });

    public int AddUserCountry(UserCountry country) => _repository.Insert(country);

    public void UpdateUserCountry(UserCountry country) => _repository.Update(country);
}

public class UserCountryQuery : BaseQuery
{
    public int? UserId { get; set; }
}