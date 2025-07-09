using System.Text.Json;
using Microsoft.JSInterop;

namespace GeoJourneyer.App.Shared.Services;

public class AuthState
{
    private const string SessionStorageKey = "auth-state";
    private const string LocalStorageKey = "auth-state-local";
    private readonly IJSRuntime _js;

    public AuthState(IJSRuntime js)
    {
        _js = js;
    }

    public string? Username { get; private set; }
    public string? Token { get; private set; }
    public DateTimeOffset? Expires { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Token) && Expires > DateTimeOffset.UtcNow;
    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        var json = await _js.InvokeAsync<string>("sessionStorage.getItem", SessionStorageKey);
        if (string.IsNullOrEmpty(json))
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
            return;
        }

        try
        {
            var stored = JsonSerializer.Deserialize<StoredAuth>(json);
            if (stored != null && DateTimeOffset.FromUnixTimeSeconds(stored.Expires) > DateTimeOffset.UtcNow)
            {
                Username = stored.Username;
                Token = stored.Token;
                Expires = DateTimeOffset.FromUnixTimeSeconds(stored.Expires);
            }
            else
            {
                await _js.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey);
                await _js.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
            }
        }
        catch
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
        }
    }

    public async Task SignInAsync(string username, string token)
    {
        Username = username;
        Token = token;
        Expires = DateTimeOffset.UtcNow.AddMinutes(30);
        await PersistAsync();
        Notify();
    }

    public async Task SignOutAsync()
    {
        Username = null;
        Token = null;
        Expires = null;
        await _js.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
        Notify();
    }

    public async Task UpdateTokenAsync(string token)
    {
        Token = token;
        Expires = DateTimeOffset.UtcNow.AddMinutes(30);
        await PersistAsync();
        Notify();
    }

    private Task PersistAsync()
    {
        if (Token == null || Username == null || Expires == null)
        {
            return Task.WhenAll(
                _js.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey).AsTask(),
                _js.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey).AsTask());
        }

        var stored = new StoredAuth
        {
            Username = Username,
            Token = Token,
            Expires = Expires.Value.ToUnixTimeSeconds()
        };
        var json = JsonSerializer.Serialize(stored);
        
        return _js.InvokeVoidAsync("sessionStorage.setItem", SessionStorageKey, json).AsTask();
    }

    private void Notify() => OnChange?.Invoke();

    private class StoredAuth
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public long Expires { get; set; }
    }
}