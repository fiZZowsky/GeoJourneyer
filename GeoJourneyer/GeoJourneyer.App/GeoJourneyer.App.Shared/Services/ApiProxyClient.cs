using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GeoJourneyer.App.Shared.Services;

public class ApiProxyClient
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public ApiProxyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        var resp = await _httpClient.PostAsJsonAsync(endpoint, payload);
        if (!resp.IsSuccessStatusCode) return default;
        return await resp.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        var resp = await _httpClient.PutAsJsonAsync(endpoint, payload);
        if (!resp.IsSuccessStatusCode) return default;
        return await resp.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
        {
            Content = JsonContent.Create(payload)
        };
        var resp = await _httpClient.SendAsync(request);
        if (!resp.IsSuccessStatusCode) return default;
        return await resp.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
    {
        var resp = await _httpClient.DeleteAsync(endpoint);
        if (!resp.IsSuccessStatusCode) return default;
        return await resp.Content.ReadFromJsonAsync<TResponse>();
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
}