namespace GeoJourneyer.Domain.Entities;

public class FriendRequest : BaseEntity
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
}