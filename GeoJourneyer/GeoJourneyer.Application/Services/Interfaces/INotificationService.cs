using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface INotificationService
{
    IEnumerable<Notification> GetForUser(int userId);
    int Add(Notification notification);
    void MarkAsRead(int id);
}