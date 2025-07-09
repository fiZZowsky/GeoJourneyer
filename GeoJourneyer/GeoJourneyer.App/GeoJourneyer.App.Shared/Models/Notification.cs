namespace GeoJourneyer.App.Shared.Models;

public enum NotificationType
{
    Message,
    FriendRequest
}

public class Notification
{
    public int Id { get; set; }
    public int? FromUserId { get; set; }
    public NotificationType Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsRead { get; set; }
}