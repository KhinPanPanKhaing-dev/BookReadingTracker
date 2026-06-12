using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Mvc.Filters;
using BookReadingTracker.Mvc.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Mvc.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly BookApiClient _bookApi;

    public BooksController(BookApiClient bookApi)
    {
        _bookApi = bookApi;
    }

    [Permission("Book.Read")]
    public async Task<IActionResult> Index()
    {
        var books = await _bookApi.GetBooksAsync();
        return View(books);
    }

    [Permission("Book.Create")]
    public IActionResult Create()
    {
        return View();
    }

    [Permission("Book.Create")]
    [HttpPost]
    public async Task<IActionResult> Create(BookDto book)
    {
        if (!ModelState.IsValid) return View(book);
        await _bookApi.CreateBookAsync(book);
        return RedirectToAction(nameof(Index));
    }

    [Permission("Book.Update")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookApi.GetBookAsync(id);
        if (book is null) return NotFound();
        return View(book);
    }

    [Permission("Book.Update")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookDto book)
    {
        if (!ModelState.IsValid) return View(book);
        await _bookApi.UpdateBookAsync(id, book);
        return RedirectToAction(nameof(Index));
    }

    [Permission("Book.Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookApi.GetBookAsync(id);
        if (book is null) return NotFound();
        return View(book);
    }

    [Permission("Book.Delete")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bookApi.DeleteBookAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
