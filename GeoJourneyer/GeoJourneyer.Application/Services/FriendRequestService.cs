using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain.Enums;
using GeoJourneyer.Domain;

namespace GeoJourneyer.Application.Services;

public class FriendRequestService : IFriendRequestService
{
    private readonly IFriendRequestRepository _repository;
    private readonly INotificationService _notifications;
    private readonly IUserRepository _users;

    public FriendRequestService(
        IFriendRequestRepository repository,
        INotificationService notifications,
        IUserRepository users)
    {
        _repository = repository;
        _notifications = notifications;
        _users = users;
    }

    public int SendRequest(int fromUserId, int toUserId)
    {
        if (_repository.GetBetweenUsers(fromUserId, toUserId) != null)
            return 0;
        var id = _repository.Insert(new FriendRequest { FromUserId = fromUserId, ToUserId = toUserId });

        var sender = _users.GetById(fromUserId);
        var text = sender != null ? $"{sender.Username} sent you a friend request" : "New friend request";
        _notifications.Add(new Notification
        {
            UserId = toUserId,
            Type = NotificationType.FriendRequest,
            Text = text
        });

        return id;
    }

    public FriendRequest? GetBetweenUsers(int fromUserId, int toUserId)
    => _repository.GetBetweenUsers(fromUserId, toUserId);

    public void UpdateStatus(int id, FriendRequestStatus status)
        => _repository.UpdateStatus(id, status);
}