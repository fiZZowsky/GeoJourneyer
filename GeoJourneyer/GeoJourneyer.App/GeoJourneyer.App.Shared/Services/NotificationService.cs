namespace GeoJourneyer.App.Shared.Services;

public class NotificationService
{
    private readonly List<Notification> _notifications = new();

    public IReadOnlyList<Notification> Notifications => _notifications;

    public event Action? OnChange;

    public void Add(Notification notification)
    {
        _notifications.Add(notification);
        Notify();
    }

    public void Remove(Notification notification)
    {
        if (_notifications.Remove(notification))
            Notify();
    }

    public void MarkAsRead(Notification notification)
    {
        if (!notification.IsRead)
        {
            notification.IsRead = true;
            Notify();
        }
    }

    public bool HasUnread => _notifications.Any(n => !n.IsRead);

    private void Notify() => OnChange?.Invoke();
}