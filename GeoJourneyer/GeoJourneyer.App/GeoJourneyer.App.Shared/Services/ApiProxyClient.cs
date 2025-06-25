using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace GeoJourneyer.App.Shared.Services;

public class ApiProxyClient
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public ApiProxyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CountryDto>?> GetCountriesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CountryDto>>("api/countries");
    }

    public async Task<string?> RegisterAsync(string username, string password)
    {
        var resp = await _httpClient.PostAsJsonAsync("api/auth/register", new { Username = username, Password = password });
        if (!resp.IsSuccessStatusCode) return null;
        var token = await resp.Content.ReadFromJsonAsync<string>();
        SetToken(token);
        return token;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var resp = await _httpClient.PostAsJsonAsync("api/auth/login", new { Username = username, Password = password });
        if (!resp.IsSuccessStatusCode) return null;
        var token = await resp.Content.ReadFromJsonAsync<string>();
        SetToken(token);
        return token;
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