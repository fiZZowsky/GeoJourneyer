using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface ITravelPlanService
{
    IEnumerable<TravelPlan> GetPlans(int userId);
    int CreatePlan(TravelPlan plan, IEnumerable<int> placeIds);
    IEnumerable<TravelPlanStop> GetPlanStops(int planId);
    IEnumerable<Place> GetPlaces(IEnumerable<int> placeIds);
    IEnumerable<TravelPlanStop> OptimizeRoute(IEnumerable<Place> places);
    void SaveStops(int planId, IEnumerable<TravelPlanStop> stops);
}