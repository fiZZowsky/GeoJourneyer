namespace GeoJourneyer.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string IsoCode { get; set; } = string.Empty;
}