using System.Threading;

namespace GeoJourneyer.App.Shared.Services;

public class LoadingService
{
    private int _counter;
    public event Action? OnChange;

    public bool IsLoading => _counter > 0;

    public void Begin()
    {
        Interlocked.Increment(ref _counter);
        Notify();
    }

    public void End()
    {
        if (Interlocked.Decrement(ref _counter) < 0)
            _counter = 0;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}