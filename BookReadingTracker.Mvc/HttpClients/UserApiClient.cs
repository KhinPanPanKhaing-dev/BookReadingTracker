using System.Net.Http.Json;
using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Mvc.HttpClients;

public class UserApiClient
{
    private readonly HttpClient _http;

    public UserApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await _http.GetFromJsonAsync<List<UserDto>>("api/users") ?? [];
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        return await _http.GetFromJsonAsync<UserDto>($"api/users/{id}");
    }

    public async Task<UserDto?> CreateUserAsync(string userName, string email, string password, string roleName)
    {
        var response = await _http.PostAsJsonAsync("api/users", new { userName, email, password, roleName });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto?> UpdateUserAsync(int id, string userName, string email, string roleName, string? password)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", new { userName, email, roleName, password });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        return response.IsSuccessStatusCode;
    }
}
