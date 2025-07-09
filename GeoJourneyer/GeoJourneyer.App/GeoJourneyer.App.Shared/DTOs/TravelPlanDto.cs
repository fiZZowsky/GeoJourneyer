namespace GeoJourneyer.App.Shared.DTOs;

public record TravelPlanDto(
    int UserId,
    int CountryId,
    string Name,
    IEnumerable<int> PlaceIds);