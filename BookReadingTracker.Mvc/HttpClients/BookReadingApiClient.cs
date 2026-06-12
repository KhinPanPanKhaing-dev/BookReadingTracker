using System.Net.Http.Json;
using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Mvc.HttpClients;

public class BookReadingApiClient
{
    private readonly HttpClient _http;

    public BookReadingApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<BookReadingDto>> GetBookReadingsAsync()
    {
        return await _http.GetFromJsonAsync<List<BookReadingDto>>("api/bookreadings") ?? [];
    }

    public async Task<BookReadingDto?> GetBookReadingAsync(int id)
    {
        return await _http.GetFromJsonAsync<BookReadingDto>($"api/bookreadings/{id}");
    }

    public async Task<List<BookReadingDto>> GetUserBookReadingsAsync(int userId)
    {
        return await _http.GetFromJsonAsync<List<BookReadingDto>>($"api/bookreadings/user/{userId}") ?? [];
    }

    public async Task<BookReadingDto?> CreateBookReadingAsync(BookReadingDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/bookreadings", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookReadingDto>();
    }

    public async Task<BookReadingDto?> UpdateBookReadingAsync(int id, BookReadingDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/bookreadings/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookReadingDto>();
    }

    public async Task<BookReadingDto?> UpdateProgressAsync(int id, int currentPage)
    {
        var response = await _http.PatchAsJsonAsync($"api/bookreadings/{id}/progress", currentPage);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookReadingDto>();
    }

    public async Task<BookReadingDto?> MarkCompletedAsync(int id)
    {
        var response = await _http.PatchAsync($"api/bookreadings/{id}/complete", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookReadingDto>();
    }

    public async Task<bool> DeleteBookReadingAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/bookreadings/{id}");
        return response.IsSuccessStatusCode;
    }
}
