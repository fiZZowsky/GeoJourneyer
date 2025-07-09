using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain;
using GeoJourneyer.Domain.Queries;
using GeoJourneyer.Infrastructure.Repositories;
using GeoJourneyer.Application.Repositories;

namespace GeoJourneyer.Application.Services;

public class UserCountryService : IUserCountryService
{
    private readonly IUserCountryRepository _repository;
    private readonly IUserRepository _userRepository;

    public UserCountryService(IUserCountryRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public IEnumerable<UserCountry> GetUserCountries(int userId)
        => _repository.GetAll(new UserCountryQuery { UserId = userId });

    public int AddUserCountry(UserCountry country) => _repository.Insert(country);

    public void UpdateUserCountry(UserCountry country) => _repository.Update(country);

    public bool DeleteUserCountry(int id)
    {
        var entry = _repository.GetById(id);
        if (entry == null) return false;

        var user = _userRepository.GetById(entry.UserId);
        if (user != null && user.CountryOfOrigin == entry.Country)
        {
            return false;
        }

        _repository.Delete(id);
        return true;
    }
}