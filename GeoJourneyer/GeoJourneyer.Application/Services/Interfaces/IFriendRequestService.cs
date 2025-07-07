namespace GeoJourneyer.Application.Services.Interfaces;

public interface IFriendRequestService
{
    int SendRequest(int fromUserId, int toUserId);
}