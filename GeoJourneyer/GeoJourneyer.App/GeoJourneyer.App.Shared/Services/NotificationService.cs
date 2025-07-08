using System.Text;
using System.Text.Json;
namespace GeoJourneyer.App.Shared.Services;

public class NotificationService
{
    private readonly ApiProxyClient _apiClient;
    private readonly AuthState _authState;
    private readonly List<Notification> _notifications = new();

    public NotificationService(ApiProxyClient apiClient, AuthState authState)
    {
        _apiClient = apiClient;
        _authState = authState;
    }

    public IReadOnlyList<Notification> Notifications => _notifications;

    public event Action? OnChange;

    public async Task LoadAsync()
    {
        var userId = GetUserId();
        if (userId == 0) return;

        var data = await _apiClient.GetAsync<IEnumerable<Notification>>($"api/notifications/{userId}");
        if (data != null)
        {
            _notifications.Clear();
            _notifications.AddRange(data);
            Notify();
        }
    }

    public async Task MarkAsReadAsync(Notification notification)
    {
        if (notification.IsRead) return;
        await _apiClient.PostAsync<object, object>($"api/notifications/{notification.Id}/read", new { });
        notification.IsRead = true;
        Notify();
    }

    public bool HasUnread => _notifications.Any(n => !n.IsRead);

    private int GetUserId()
    {
        var token = _authState.Token;
        if (string.IsNullOrEmpty(token)) return 0;

        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return 0;
            var payload = parts[1].PadRight(parts[1].Length + (4 - parts[1].Length % 4) % 4, '=');
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("sub", out var sub))
            {
                return sub.GetInt32();
            }
        }
        catch
        {
        }
        return 0;
    }

    public void MarkAsRead(Notification notification)
    {
        if (!notification.IsRead)
        {
            notification.IsRead = true;
            Notify();
        }
    }

    private void Notify() => OnChange?.Invoke();
}