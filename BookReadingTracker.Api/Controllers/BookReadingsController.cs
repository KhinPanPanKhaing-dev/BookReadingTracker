using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookReadingsController : ControllerBase
{
    private readonly IBookReadingService _bookReadingService;

    public BookReadingsController(IBookReadingService bookReadingService)
    {
        _bookReadingService = bookReadingService;
    }

    [HttpGet]
    public IActionResult GetBookReadings()
    {
        var readings = _bookReadingService.GetBookReadings();
        return Ok(readings);
    }

    [HttpGet("{id}")]
    public IActionResult GetBookReading(int id)
    {
        var reading = _bookReadingService.GetBookReading(id);
        if (reading is null) return NotFound();
        return Ok(reading);
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetUserBookReadings(int userId)
    {
        var readings = _bookReadingService.GetUserBookReadings(userId);
        return Ok(readings);
    }

    [HttpPost]
    public IActionResult CreateBookReading([FromBody] BookReadingDto dto)
    {
        var created = _bookReadingService.CreateBookReading(dto);
        return CreatedAtAction(nameof(GetBookReading), new { id = created.BookReadingId }, created);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBookReading(int id, [FromBody] BookReadingDto dto)
    {
        var updated = _bookReadingService.UpdateBookReading(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpPatch("{id}/progress")]
    public IActionResult UpdateProgress(int id, [FromBody] ProgressRequest request)
    {
        var updated = _bookReadingService.UpdateProgress(id, request.CurrentPage);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpPatch("{id}/complete")]
    public IActionResult MarkCompleted(int id)
    {
        var updated = _bookReadingService.MarkCompleted(id);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBookReading(int id)
    {
        var deleted = _bookReadingService.DeleteBookReading(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

public class ProgressRequest
{
    public int CurrentPage { get; set; }
}
