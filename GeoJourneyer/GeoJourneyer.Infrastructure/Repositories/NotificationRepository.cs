using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    protected override string TableName => "Notifications";

    public NotificationRepository(DatabaseContext context) : base(context)
    {
    }

    public void MarkAsRead(int id)
    {
        using var connection = Context.CreateConnection();
        connection.Execute($"UPDATE {TableName} SET IsRead = 1 WHERE Id = @id", new { id });
    }
}