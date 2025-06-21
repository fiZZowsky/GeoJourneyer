using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Application.Repositories;

namespace GeoJourneyer.Application.Services;

public class PlaceService : IPlaceService
{
    private readonly IPlaceRepository _repository;

    public PlaceService(IPlaceRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Place> GetAll() => _repository.GetAll();

    public IEnumerable<Place> GetByCountry(int countryId) => _repository.GetByCountry(countryId);

    public int AddPlace(Place place) => _repository.Insert(place);
}