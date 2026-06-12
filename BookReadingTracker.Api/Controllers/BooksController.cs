using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _bookService.GetBooks();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetBook(int id)
    {
        var book = _bookService.GetBook(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody] BookDto book)
    {
        var created = _bookService.CreateBook(book);
        return CreatedAtAction(nameof(GetBook), new { id = created.BookId }, created);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] BookDto book)
    {
        var updated = _bookService.UpdateBook(id, book);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var deleted = _bookService.DeleteBook(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
