using System.Net.Http.Json;
using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Mvc.HttpClients;

public class BookApiClient
{
    private readonly HttpClient _http;

    public BookApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<BookDto>> GetBooksAsync()
    {
        return await _http.GetFromJsonAsync<List<BookDto>>("api/books") ?? [];
    }

    public async Task<BookDto?> GetBookAsync(int id)
    {
        return await _http.GetFromJsonAsync<BookDto>($"api/books/{id}");
    }

    public async Task<BookDto?> CreateBookAsync(BookDto book)
    {
        var response = await _http.PostAsJsonAsync("api/books", book);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookDto>();
    }

    public async Task<BookDto?> UpdateBookAsync(int id, BookDto book)
    {
        var response = await _http.PutAsJsonAsync($"api/books/{id}", book);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookDto>();
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/books/{id}");
        return response.IsSuccessStatusCode;
    }
}
