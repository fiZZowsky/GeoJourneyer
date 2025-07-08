using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    void MarkAsRead(int id);
}