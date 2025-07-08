using GeoJourneyer.Domain.Enums;

namespace GeoJourneyer.Domain.Entities;

public class Notification : BaseEntity
{
    public int? FromUserId { get; set; }
    public int UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsRead { get; set; }
}