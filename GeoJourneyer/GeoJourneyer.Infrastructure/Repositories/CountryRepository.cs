using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    protected override string TableName => "Countries";

    public CountryRepository(DatabaseContext context) : base(context)
    {
    }
}