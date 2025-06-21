using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Repositories;

namespace GeoJourneyer.Application.Repositories;

public interface ITravelPlanRepository : IBaseRepository<TravelPlan>
{
    IEnumerable<TravelPlanStop> GetStops(int planId);
    void SaveStops(int planId, IEnumerable<TravelPlanStop> stops);
}