namespace GeoJourneyer.App.Shared.Services;

public record UserCountryDto(
    int Id,
    int UserId,
    string Country,
    CountryStatus Status);