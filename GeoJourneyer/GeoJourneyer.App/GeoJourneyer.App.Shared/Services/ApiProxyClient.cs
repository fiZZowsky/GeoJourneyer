using System.Net.Http;
using System.Net.Http.Json;

namespace GeoJourneyer.App.Shared.Services;

public class ApiProxyClient
{
    private readonly HttpClient _httpClient;

    public ApiProxyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CountryDto>?> GetCountriesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CountryDto>>("api/countries");
    }

    public async Task<int?> RegisterAsync(string username, string password)
    {
        var resp = await _httpClient.PostAsJsonAsync("api/auth/register", new { Username = username, Password = password });
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<int>();
    }

    public async Task<int?> LoginAsync(string username, string password)
    {
        var resp = await _httpClient.PostAsJsonAsync("api/auth/login", new { Username = username, Password = password });
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<int>();
    }
}