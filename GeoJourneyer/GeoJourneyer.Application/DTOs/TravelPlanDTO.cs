namespace GeoJourneyer.Application.DTOs;

using System.Collections.Generic;

public record TravelPlanDto(
    int UserId, 
    int CountryId, 
    string Name, 
    IEnumerable<int> PlaceIds
);