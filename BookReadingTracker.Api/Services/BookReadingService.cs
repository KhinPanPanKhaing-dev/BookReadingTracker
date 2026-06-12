using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.Constants;
using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReadingTracker.Api.Services;

public class BookReadingService : IBookReadingService
{
    private readonly AppDbContext _db;

    public BookReadingService(AppDbContext db)
    {
        _db = db;
    }

    public List<BookReadingDto> GetBookReadings()
    {
        return _db.BookReadings
            .AsNoTracking()
            .Include(x => x.Book)
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new BookReadingDto
            {
                BookReadingId = x.BookReadingId,
                UserId = x.UserId,
                BookId = x.BookId,
                CurrentPage = x.CurrentPage,
                Status = x.Status,
                StartedDate = x.StartedDate,
                CompletedDate = x.CompletedDate,
                BookTitle = x.Book.Title,
                UserName = x.User.UserName,
                Author = x.Book.Author,
                TotalPages = x.Book.TotalPages
            })
            .ToList();
    }

    public BookReadingDto? GetBookReading(int id)
    {
        var x = _db.BookReadings
            .AsNoTracking()
            .Include(x => x.Book)
            .Include(x => x.User)
            .FirstOrDefault(r => r.BookReadingId == id);

        if (x is null) return null;

        return new BookReadingDto
        {
            BookReadingId = x.BookReadingId,
            UserId = x.UserId,
            BookId = x.BookId,
            CurrentPage = x.CurrentPage,
            Status = x.Status,
            StartedDate = x.StartedDate,
            CompletedDate = x.CompletedDate,
            BookTitle = x.Book.Title,
            UserName = x.User.UserName,
            Author = x.Book.Author,
            TotalPages = x.Book.TotalPages
        };
    }

    public BookReadingDto CreateBookReading(BookReadingDto dto)
    {
        var entity = new BookReading
        {
            UserId = dto.UserId,
            BookId = dto.BookId,
            CurrentPage = dto.CurrentPage,
            Status = StatusConstants.NotStarted,
            StartedDate = null,
            CompletedDate = null,
            CreatedBy = dto.UserName ?? "system",
            CreatedDate = DateTime.UtcNow
        };

        _db.BookReadings.Add(entity);
        _db.SaveChanges();

        dto.BookReadingId = entity.BookReadingId;
        dto.Status = entity.Status;
        dto.CreatedDate = entity.CreatedDate;
        return dto;
    }

    public BookReadingDto? UpdateBookReading(int id, BookReadingDto dto)
    {
        var entity = _db.BookReadings
            .Include(x => x.Book)
            .Include(x => x.User)
            .FirstOrDefault(r => r.BookReadingId == id);

        if (entity is null) return null;

        entity.UserId = dto.UserId;
        entity.BookId = dto.BookId;
        entity.CurrentPage = dto.CurrentPage;
        entity.Status = dto.Status ?? entity.Status;
        entity.StartedDate = dto.StartedDate;
        entity.CompletedDate = dto.CompletedDate;
        entity.ModifiedBy = dto.UserName;
        entity.ModifiedDate = DateTime.UtcNow;

        _db.SaveChanges();

        dto.BookTitle = entity.Book.Title;
        dto.UserName = entity.User.UserName;
        return dto;
    }

    public bool DeleteBookReading(int id)
    {
        var entity = _db.BookReadings.Find(id);
        if (entity is null) return false;

        _db.BookReadings.Remove(entity);
        _db.SaveChanges();
        return true;
    }

    public List<BookReadingDto> GetUserBookReadings(int userId)
    {
        return _db.BookReadings
            .AsNoTracking()
            .Include(x => x.Book)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new BookReadingDto
            {
                BookReadingId = x.BookReadingId,
                UserId = x.UserId,
                BookId = x.BookId,
                CurrentPage = x.CurrentPage,
                Status = x.Status,
                StartedDate = x.StartedDate,
                CompletedDate = x.CompletedDate,
                BookTitle = x.Book.Title,
                Author = x.Book.Author,
                TotalPages = x.Book.TotalPages
            })
            .ToList();
    }

    public BookReadingDto? UpdateProgress(int id, int currentPage)
    {
        var entity = _db.BookReadings
            .Include(x => x.Book)
            .Include(x => x.User)
            .FirstOrDefault(r => r.BookReadingId == id);

        if (entity is null) return null;

        entity.CurrentPage = currentPage;
        entity.Status = currentPage >= entity.Book.TotalPages
            ? StatusConstants.Completed
            : StatusConstants.InProgress;

        if (entity.Status == StatusConstants.InProgress && entity.StartedDate is null)
            entity.StartedDate = DateTime.UtcNow;

        if (entity.Status == StatusConstants.Completed)
            entity.CompletedDate = DateTime.UtcNow;

        entity.ModifiedDate = DateTime.UtcNow;
        _db.SaveChanges();

        return new BookReadingDto
        {
            BookReadingId = entity.BookReadingId,
            UserId = entity.UserId,
            BookId = entity.BookId,
            CurrentPage = entity.CurrentPage,
            Status = entity.Status,
            StartedDate = entity.StartedDate,
            CompletedDate = entity.CompletedDate,
            BookTitle = entity.Book.Title,
            UserName = entity.User.UserName,
            Author = entity.Book.Author,
            TotalPages = entity.Book.TotalPages
        };
    }

    public BookReadingDto? MarkCompleted(int id)
    {
        return UpdateProgress(id, int.MaxValue);
    }
}
