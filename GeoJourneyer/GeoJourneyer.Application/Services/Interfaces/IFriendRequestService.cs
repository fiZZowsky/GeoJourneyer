using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IFriendRequestService
{
    int SendRequest(int fromUserId, int toUserId);
    FriendRequest? GetBetweenUsers(int fromUserId, int toUserId);
    void UpdateStatus(int id, FriendRequestStatus status);
}