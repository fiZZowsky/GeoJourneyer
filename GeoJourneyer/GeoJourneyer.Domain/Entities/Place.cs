namespace GeoJourneyer.Domain.Entities;

public class Place : BaseEntity
{
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
}