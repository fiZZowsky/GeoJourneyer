namespace GeoJourneyer.App.Shared.Services;

public record TravelPlanDto(
    int UserId,
    int CountryId,
    string Name,
    IEnumerable<int> PlaceIds);