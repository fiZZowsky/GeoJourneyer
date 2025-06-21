namespace GeoJourneyer.Domain;

public class UserCountry : BaseEntity
{
    public int UserId { get; set; }
    public string Country { get; set; } = string.Empty;
    public CountryStatus Status { get; set; }
}
