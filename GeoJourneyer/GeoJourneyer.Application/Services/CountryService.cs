using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Repositories;

namespace GeoJourneyer.Application.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;

    public CountryService(ICountryRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Country> GetAll() => _repository.GetAll();

    public Country? GetById(int id) => _repository.GetById(id);

    public int AddCountry(Country country) => _repository.Insert(country);
}