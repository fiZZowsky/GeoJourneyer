using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services;

public class FriendRequestService : IFriendRequestService
{
    private readonly IFriendRequestRepository _repository;

    public FriendRequestService(IFriendRequestRepository repository)
    {
        _repository = repository;
    }

    public int SendRequest(int fromUserId, int toUserId)
    {
        if (_repository.GetBetweenUsers(fromUserId, toUserId) != null)
            return 0;
        return _repository.Insert(new FriendRequest { FromUserId = fromUserId, ToUserId = toUserId });
    }
}