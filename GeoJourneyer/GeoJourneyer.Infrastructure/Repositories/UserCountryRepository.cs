using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class UserCountryRepository : BaseRepository<UserCountry>, IUserCountryRepository
{
    protected override string TableName => "UserCountries";

    public UserCountryRepository(DatabaseContext context) : base(context)
    {
    }
}
