using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Domain.Interfaces;

public interface IBookService
{
    List<BookDto> GetBooks();
    BookDto? GetBook(int id);
    BookDto CreateBook(BookDto book);
    BookDto? UpdateBook(int id, BookDto book);
    bool DeleteBook(int id);
}
