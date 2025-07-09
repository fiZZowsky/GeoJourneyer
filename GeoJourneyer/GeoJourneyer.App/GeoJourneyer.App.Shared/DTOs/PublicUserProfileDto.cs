namespace GeoJourneyer.App.Shared.DTOs;

public class PublicUserProfileDto
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string CountryOfOrigin { get; set; } = string.Empty;
    public int VisitedCount { get; set; }
    public double VisitedPercent { get; set; }
    public string? PhotoUrl { get; set; }
}