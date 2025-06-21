using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IPlaceService
{
    IEnumerable<Place> GetByCountry(int countryId);
    IEnumerable<Place> GetAll();
    int AddPlace(Place place);
}