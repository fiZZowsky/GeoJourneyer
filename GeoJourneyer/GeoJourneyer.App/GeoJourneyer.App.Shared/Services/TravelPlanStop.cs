namespace GeoJourneyer.App.Shared.Services;

public class TravelPlanStop
{
    public int Id { get; set; }
    public int TravelPlanId { get; set; }
    public int PlaceId { get; set; }
    public int Order { get; set; }
}