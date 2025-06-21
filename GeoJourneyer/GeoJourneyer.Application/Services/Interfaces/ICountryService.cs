using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface ICountryService
{
    IEnumerable<Country> GetAll();
    Country? GetById(int id);
    int AddCountry(Country country);
}