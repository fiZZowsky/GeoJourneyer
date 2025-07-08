using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace GeoJourneyer.App.Shared.Services;

public class ApiProxyClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthState _authState;
    private readonly LoadingService _loading;
    private string? _token;

    public ApiProxyClient(HttpClient httpClient, AuthState authState, LoadingService loading)
    {
        _httpClient = httpClient;
        _authState = authState;
        _loading = loading;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }
        finally
        {
            _loading.End();
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            var resp = await _httpClient.PostAsJsonAsync(endpoint, payload);
            if (!resp.IsSuccessStatusCode) return default;
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }
        finally
        {
            _loading.End();
        }
    }

    public async Task<TResponse?> PostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent content)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            var resp = await _httpClient.PostAsync(endpoint, content);
            if (!resp.IsSuccessStatusCode) return default;
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }
        finally
        {
            _loading.End();
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            var resp = await _httpClient.PutAsJsonAsync(endpoint, payload);
            if (!resp.IsSuccessStatusCode) return default;
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }
        finally
        {
            _loading.End();
        }
    }

    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
            {
                Content = JsonContent.Create(payload)
            };
            var resp = await _httpClient.SendAsync(request);
            if (!resp.IsSuccessStatusCode) return default;
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }
        finally
        {
            _loading.End();
        }
    }

    public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
    {
        await EnsureTokenAsync();
        _loading.Begin();
        try
        {
            var resp = await _httpClient.DeleteAsync(endpoint);
            if (!resp.IsSuccessStatusCode) return default;
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }
        finally
        {
            _loading.End();
        }
    }

    public void SetToken(string? token)
    {
        _token = token;
        if (string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task EnsureTokenAsync()
    {
        if (!_authState.IsLoggedIn)
            return;

        if (string.IsNullOrEmpty(_token) && !string.IsNullOrEmpty(_authState.Token))
        {
            SetToken(_authState.Token);
        }

        if (_authState.Expires <= DateTimeOffset.UtcNow)
        {
            await _authState.SignOutAsync();
            SetToken(null);
            return;
        }

        if ((_authState.Expires - DateTimeOffset.UtcNow)?.TotalMinutes <= 5)
        {
            var resp = await _httpClient.PostAsync("api/auth/refresh", null);
            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authState.SignOutAsync();
                SetToken(null);
                return;
            }

            if (resp.IsSuccessStatusCode)
            {
                var tokenDto = await resp.Content.ReadFromJsonAsync<AuthTokenDto>();
                if (tokenDto?.Token != null)
                {
                    await _authState.UpdateTokenAsync(tokenDto.Token);
                    SetToken(tokenDto.Token);
                }
            }
        }
    }
}