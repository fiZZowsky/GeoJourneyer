using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class TravelPlanRepository : BaseRepository<TravelPlan>, ITravelPlanRepository
{
    protected override string TableName => "TravelPlans";

    public TravelPlanRepository(DatabaseContext context) : base(context)
    {
    }

    public IEnumerable<TravelPlanStop> GetStops(int planId)
    {
        using var connection = Context.CreateConnection();
        var sql = "SELECT * FROM TravelPlanStops WHERE TravelPlanId = @planId ORDER BY [Order]";
        return connection.Query<TravelPlanStop>(sql, new { planId });
    }

    public void SaveStops(int planId, IEnumerable<TravelPlanStop> stops)
    {
        using var connection = Context.CreateConnection();
        connection.Execute("DELETE FROM TravelPlanStops WHERE TravelPlanId = @planId", new { planId });
        foreach (var stop in stops)
        {
            connection.Execute(
                "INSERT INTO TravelPlanStops (TravelPlanId, PlaceId, [Order]) VALUES (@TravelPlanId, @PlaceId, @Order)",
                new { TravelPlanId = planId, stop.PlaceId, stop.Order });
        }
    }

    public IEnumerable<TravelPlan> GetInfos(int userId)
    {
        using var connection = Context.CreateConnection();
        var sql = $"SELECT * FROM {TableName} WHERE UserId = @userId";
        return connection.Query<TravelPlan>(sql, new { userId });
    }
}