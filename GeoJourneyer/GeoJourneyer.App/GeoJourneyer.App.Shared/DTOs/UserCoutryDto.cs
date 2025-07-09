using GeoJourneyer.App.Shared.Enums;

namespace GeoJourneyer.App.Shared.DTOs;

public record UserCountryDto(
    int Id,
    int UserId,
    string Country,
    CountryStatus Status);