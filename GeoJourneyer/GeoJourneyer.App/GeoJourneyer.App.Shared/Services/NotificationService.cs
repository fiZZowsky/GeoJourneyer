using System.Text;
using System.Text.Json;
namespace GeoJourneyer.App.Shared.Services;

public class NotificationService : IDisposable
{
    private readonly ApiProxyClient _apiClient;
    private readonly AuthState _authState;
    private readonly List<Notification> _notifications = new();
    private System.Threading.Timer? _timer;

    public NotificationService(ApiProxyClient apiClient, AuthState authState)
    {
        _apiClient = apiClient;
        _authState = authState;
        _authState.OnChange += AuthChanged;
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
            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
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

    public void Remove(Notification notification)
    {
        if (_notifications.Remove(notification))
        {
            Notify();
        }
    }

    public async Task InitializeAsync()
    {
        if (_authState.IsLoggedIn)
        {
            await LoadAsync();
            StartTimer();
        }
    }

    private void AuthChanged()
    {
        if (_authState.IsLoggedIn)
        {
            _ = LoadAsync();
            StartTimer();
        }
        else
        {
            StopTimer();
            _notifications.Clear();
            Notify();
        }
    }

    private void StartTimer()
    {
        _timer?.Dispose();
        _timer = new System.Threading.Timer(async _ => await LoadAsync(), null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }

    private void StopTimer()
    {
        _timer?.Dispose();
        _timer = null;
    }

    private void Notify() => OnChange?.Invoke();

    public void Dispose()
    {
        _authState.OnChange -= AuthChanged;
        StopTimer();
    }
}