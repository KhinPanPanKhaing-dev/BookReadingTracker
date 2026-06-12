using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Mvc.Filters;
using BookReadingTracker.Mvc.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Mvc.Controllers;

[Authorize]
public class BookReadingsController : Controller
{
    private readonly BookReadingApiClient _readingApi;
    private readonly BookApiClient _bookApi;
    private readonly UserApiClient _userApi;

    public BookReadingsController(BookReadingApiClient readingApi, BookApiClient bookApi, UserApiClient userApi)
    {
        _readingApi = readingApi;
        _bookApi = bookApi;
        _userApi = userApi;
    }

    [Permission("Reading.Read")]
    public async Task<IActionResult> Index()
    {
        var readings = await _readingApi.GetBookReadingsAsync();
        return View(readings);
    }

    [Permission("Reading.Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Books = await _bookApi.GetBooksAsync();
        ViewBag.Users = await _userApi.GetUsersAsync();
        return View();
    }

    [Permission("Reading.Create")]
    [HttpPost]
    public async Task<IActionResult> Create(BookReadingDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Books = await _bookApi.GetBooksAsync();
            ViewBag.Users = await _userApi.GetUsersAsync();
            return View(dto);
        }
        await _readingApi.CreateBookReadingAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Permission("Reading.Update")]
    public async Task<IActionResult> Edit(int id)
    {
        var reading = await _readingApi.GetBookReadingAsync(id);
        if (reading is null) return NotFound();
        ViewBag.Books = await _bookApi.GetBooksAsync();
        ViewBag.Users = await _userApi.GetUsersAsync();
        return View(reading);
    }

    [Permission("Reading.Update")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookReadingDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Books = await _bookApi.GetBooksAsync();
            ViewBag.Users = await _userApi.GetUsersAsync();
            return View(dto);
        }
        await _readingApi.UpdateBookReadingAsync(id, dto);
        return RedirectToAction(nameof(Index));
    }

    [Permission("Reading.Update")]
    public async Task<IActionResult> UpdateProgress(int id)
    {
        var reading = await _readingApi.GetBookReadingAsync(id);
        if (reading is null) return NotFound();
        return View(reading);
    }

    [Permission("Reading.Update")]
    [HttpPost]
    public async Task<IActionResult> UpdateProgress(int id, int currentPage)
    {
        await _readingApi.UpdateProgressAsync(id, currentPage);
        return RedirectToAction(nameof(Index));
    }

    [Permission("Reading.Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var reading = await _readingApi.GetBookReadingAsync(id);
        if (reading is null) return NotFound();
        return View(reading);
    }

    [Permission("Reading.Delete")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _readingApi.DeleteBookReadingAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
