namespace GeoJourneyer.App.Shared.Models;

public class TravelPlanStop
{
    public int Id { get; set; }
    public int TravelPlanId { get; set; }
    public int PlaceId { get; set; }
    public int Order { get; set; }
}