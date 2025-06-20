namespace GeoJourneyer.Domain;

public class UserCountry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Country { get; set; } = string.Empty;
    public CountryStatus Status { get; set; }
}
