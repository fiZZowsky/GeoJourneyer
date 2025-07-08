namespace GeoJourneyer.App.Shared.Services;

public record PlaceDto(
    int Id,
    int CountryId,
    string Name,
    double Latitude,
    double Longitude,
    string? Description);