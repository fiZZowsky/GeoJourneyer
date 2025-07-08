using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.DTOs;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Infrastructure.Repositories;

public interface ITravelPlanRepository : IBaseRepository<TravelPlan>
{
    IEnumerable<TravelPlanStop> GetStops(int planId);
    void SaveStops(int planId, IEnumerable<TravelPlanStop> stops);
    IEnumerable<TravelPlanInfoDTO> GetInfos(int userId);
}