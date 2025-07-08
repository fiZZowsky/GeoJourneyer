using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Domain.Queries;

namespace GeoJourneyer.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Notification> GetForUser(int userId)
        => _repository.GetAll(new NotificationQuery { UserId = userId });

    public int Add(Notification notification)
        => _repository.Insert(notification);

    public void MarkAsRead(int id)
        => _repository.MarkAsRead(id);
}