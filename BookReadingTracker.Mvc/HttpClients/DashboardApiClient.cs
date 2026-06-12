using System.Net.Http.Json;
using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Mvc.HttpClients;

public class DashboardApiClient
{
    private readonly HttpClient _http;

    public DashboardApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        return await _http.GetFromJsonAsync<DashboardDto>("api/dashboard") ?? new DashboardDto();
    }
}
