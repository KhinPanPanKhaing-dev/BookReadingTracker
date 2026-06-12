using System.Net.Http.Json;
using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Mvc.HttpClients;

public class AuthApiClient
{
    private readonly HttpClient _http;

    public AuthApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthResultDto?> LoginAsync(LoginDto login)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", login);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResultDto>();
    }

    public async Task<AuthResultDto?> RegisterAsync(RegisterDto register)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", register);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResultDto>();
    }
}
