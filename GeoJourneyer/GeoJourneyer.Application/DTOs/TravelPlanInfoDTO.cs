namespace GeoJourneyer.Application.DTOs;

public class TravelPlanInfoDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int PlaceCount { get; set; }
}