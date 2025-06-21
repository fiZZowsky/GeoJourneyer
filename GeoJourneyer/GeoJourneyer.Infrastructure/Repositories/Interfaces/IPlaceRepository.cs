using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Repositories;

namespace GeoJourneyer.Application.Repositories;

public interface IPlaceRepository : IBaseRepository<Place>
{
    IEnumerable<Place> GetByCountry(int countryId);
}