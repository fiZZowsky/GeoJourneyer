namespace GeoJourneyer.Domain.Entities;

public class TravelPlanStop : BaseEntity
{
    public int TravelPlanId { get; set; }
    public int PlaceId { get; set; }
    public int Order { get; set; }
}