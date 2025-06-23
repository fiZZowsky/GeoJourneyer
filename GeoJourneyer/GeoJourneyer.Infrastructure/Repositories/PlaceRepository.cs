using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain.Queries;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
{
    protected override string TableName => "Places";

    public PlaceRepository(DatabaseContext context) : base(context)
    {
    }

    public IEnumerable<Place> GetByCountry(int countryId)
    {
        using var connection = Context.CreateConnection();
        return connection.Query<Place>($"SELECT * FROM {TableName} WHERE CountryId = @countryId", new { countryId });
    }

    public IEnumerable<Place> GetAll(BaseQuery query = null)
    {
        throw new NotImplementedException();
    }
}