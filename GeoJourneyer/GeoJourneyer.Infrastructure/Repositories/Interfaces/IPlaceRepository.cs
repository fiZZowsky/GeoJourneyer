using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Infrastructure.Repositories;

public interface IPlaceRepository : IBaseRepository<Place>
{
    IEnumerable<Place> GetByCountry(int countryId);
}