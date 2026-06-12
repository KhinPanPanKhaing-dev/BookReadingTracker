using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReadingTracker.Api.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;

    public BookService(AppDbContext db)
    {
        _db = db;
    }

    public List<BookDto> GetBooks()
    {
        return _db.Books
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new BookDto
            {
                BookId = x.BookId,
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                TotalPages = x.TotalPages,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate
            })
            .ToList();
    }

    public BookDto? GetBook(int id)
    {
        var x = _db.Books.AsNoTracking().FirstOrDefault(b => b.BookId == id);
        if (x is null) return null;

        return new BookDto
        {
            BookId = x.BookId,
            Title = x.Title,
            Author = x.Author,
            Description = x.Description,
            TotalPages = x.TotalPages,
            CreatedBy = x.CreatedBy,
            CreatedDate = x.CreatedDate,
            ModifiedBy = x.ModifiedBy,
            ModifiedDate = x.ModifiedDate
        };
    }

    public BookDto CreateBook(BookDto book)
    {
        var entity = new Book
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            TotalPages = book.TotalPages,
            CreatedBy = "Admin",
            CreatedDate = DateTime.UtcNow
        };

        _db.Books.Add(entity);
        _db.SaveChanges();

        book.BookId = entity.BookId;
        book.CreatedDate = entity.CreatedDate;
        return book;
    }

    public BookDto? UpdateBook(int id, BookDto book)
    {
        var entity = _db.Books.Find(id);
        if (entity is null) return null;

        entity.Title = book.Title;
        entity.Author = book.Author;
        entity.Description = book.Description;
        entity.TotalPages = book.TotalPages;
        entity.ModifiedBy = book.ModifiedBy;
        entity.ModifiedDate = DateTime.UtcNow;

        _db.SaveChanges();

        book.BookId = entity.BookId;
        book.CreatedDate = entity.CreatedDate;
        book.CreatedBy = entity.CreatedBy;
        book.ModifiedDate = entity.ModifiedDate;
        return book;
    }

    public bool DeleteBook(int id)
    {
        var entity = _db.Books.Include(x => x.BookReadings).FirstOrDefault(x => x.BookId == id);
        if (entity is null) return false;

        _db.BookReadings.RemoveRange(entity.BookReadings);
        _db.Books.Remove(entity);
        _db.SaveChanges();
        return true;
    }
}
