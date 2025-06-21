namespace GeoJourneyer.Domain.Entities;

public class TravelPlan : BaseEntity
{
    public int UserId { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
}