namespace GeoJourneyer.App.Shared.DTOs;

public record PlaceDto(
    int Id,
    int CountryId,
    string Name,
    double Latitude,
    double Longitude,
    string? Description);