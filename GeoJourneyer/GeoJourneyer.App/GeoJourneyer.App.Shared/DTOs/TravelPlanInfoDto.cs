namespace GeoJourneyer.App.Shared.DTOs;

public class TravelPlanInfoDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int PlaceCount { get; set; }
}