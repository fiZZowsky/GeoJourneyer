using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain;

namespace GeoJourneyer.Application.Repositories;

public interface IFriendRequestRepository : IBaseRepository<FriendRequest>
{
    FriendRequest? GetBetweenUsers(int fromUserId, int toUserId);
    void UpdateStatus(int id, FriendRequestStatus status);
}