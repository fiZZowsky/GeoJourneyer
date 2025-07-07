using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Repositories;

public interface IFriendRequestRepository : IBaseRepository<FriendRequest>
{
    FriendRequest? GetBetweenUsers(int fromUserId, int toUserId);
}