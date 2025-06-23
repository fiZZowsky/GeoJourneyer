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
        return await _httpClient.GetFromJsonAsync<IEnumerable<CountryDto>>("countries");
    }
}