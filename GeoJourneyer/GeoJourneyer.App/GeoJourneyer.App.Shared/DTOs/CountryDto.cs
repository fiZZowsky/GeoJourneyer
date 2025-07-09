namespace GeoJourneyer.App.Shared.DTOs;

public record CountryDto(
    int Id, 
    string Name, 
    string IsoCode,
    string Language
);