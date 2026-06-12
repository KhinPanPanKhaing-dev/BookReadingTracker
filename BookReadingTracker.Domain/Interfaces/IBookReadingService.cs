using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Domain.Interfaces;

public interface IBookReadingService
{
    List<BookReadingDto> GetBookReadings();
    BookReadingDto? GetBookReading(int id);
    BookReadingDto CreateBookReading(BookReadingDto bookReading);
    BookReadingDto? UpdateBookReading(int id, BookReadingDto bookReading);
    bool DeleteBookReading(int id);
    List<BookReadingDto> GetUserBookReadings(int userId);
    BookReadingDto? UpdateProgress(int id, int currentPage);
    BookReadingDto? MarkCompleted(int id);
}
