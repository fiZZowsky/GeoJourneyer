using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain.Queries;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    protected override string TableName => "Countries";

    public CountryRepository(DatabaseContext context) : base(context)
    {
    }

    public IEnumerable<Country> GetAll(BaseQuery query = null)
    {
        throw new NotImplementedException();
    }
}