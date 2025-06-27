namespace GeoJourneyer.App.Shared.Services;

public class AuthState
{
    public string? Username { get; private set; }
    public string? Token { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public event Action? OnChange;

    public void SignIn(string username, string token)
    {
        Username = username;
        Token = token;
        Notify();
    }

    public void SignOut()
    {
        Username = null;
        Token = null;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}