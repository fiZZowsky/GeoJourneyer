using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain.Queries;
using GeoJourneyer.Infrastructure.Repositories;
using GeoJourneyer.Application.DTOs;

namespace GeoJourneyer.Application.Services;

public class TravelPlanService : ITravelPlanService
{
    private readonly ITravelPlanRepository _planRepository;
    private readonly IPlaceRepository _placeRepository;

    public TravelPlanService(ITravelPlanRepository planRepository, IPlaceRepository placeRepository)
    {
        _planRepository = planRepository;
        _placeRepository = placeRepository;
    }

    public IEnumerable<TravelPlanInfoDTO> GetPlans(int userId)
        => _planRepository.GetInfos(userId);

    public int CreatePlan(TravelPlan plan, IEnumerable<int> placeIds)
    {
        if (placeIds == null || !placeIds.Any())
        {
            throw new ArgumentException("At least one place is required", nameof(placeIds));
        }

        plan.CreatedAt = DateTime.UtcNow;
        var id = _planRepository.Insert(plan);
        var order = 0;
        var stops = placeIds.Select(pid => new TravelPlanStop
        {
            TravelPlanId = id,
            PlaceId = pid,
            Order = order++
        });
        _planRepository.SaveStops(id, stops);
        return id;
    }

    public IEnumerable<TravelPlanStop> GetPlanStops(int planId) => _planRepository.GetStops(planId);

    public IEnumerable<Place> GetPlaces(IEnumerable<int> placeIds)
        => placeIds.Select(id => _placeRepository.GetById(id)!).Where(p => p != null)!;

    public IEnumerable<TravelPlanStop> OptimizeRoute(IEnumerable<Place> places)
    {
        var remaining = places.ToList();
        var result = new List<TravelPlanStop>();
        Place? current = remaining.FirstOrDefault();
        var order = 0;
        while (current != null)
        {
            remaining.Remove(current);
            result.Add(new TravelPlanStop { PlaceId = current.Id, Order = order++ });
            current = remaining.OrderBy(p => Distance(current, p)).FirstOrDefault();
        }
        return result;
    }

    public void SaveStops(int planId, IEnumerable<TravelPlanStop> stops) => _planRepository.SaveStops(planId, stops);

    private static double Distance(Place a, Place b)
    {
        var dLat = (a.Latitude - b.Latitude) * Math.PI / 180.0;
        var dLon = (a.Longitude - b.Longitude) * Math.PI / 180.0;
        var lat1 = a.Latitude * Math.PI / 180.0;
        var lat2 = b.Latitude * Math.PI / 180.0;
        var sinDLat = Math.Sin(dLat / 2);
        var sinDLon = Math.Sin(dLon / 2);
        var aa = sinDLat * sinDLat + sinDLon * sinDLon * Math.Cos(lat1) * Math.Cos(lat2);
        var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(aa)));
        return 6371 * c; // km
    }
}