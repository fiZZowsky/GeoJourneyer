using GeoJourneyer.Infrastructure.Persistance;
using GeoJourneyer.Infrastructure.Repositories.Interfaces;

namespace GeoJourneyer.Infrastructure.Repositories;

public class UserCountryRepository : BaseRepository<UserCountry>, IUserCountryRepository
{
    protected override string TableName => "UserCountries";

    public UserCountryRepository(DatabaseContext context) : base(context)
    {
    }
}
