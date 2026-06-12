using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.Constants;
using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReadingTracker.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public DashboardDto GetDashboard()
    {
        var totalBooks = _db.Books.Count();
        var totalUsers = _db.Users.Count();
        var totalReadings = _db.BookReadings.Count();
        var completedReadings = _db.BookReadings.Count(x => x.Status == StatusConstants.Completed);
        var inProgressReadings = _db.BookReadings.Count(x => x.Status == StatusConstants.InProgress);
        var notStartedReadings = _db.BookReadings.Count(x => x.Status == StatusConstants.NotStarted);

        var recentActivities = _db.BookReadings
            .AsNoTracking()
            .Include(x => x.Book)
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedDate)
            .Take(10)
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

        return new DashboardDto
        {
            TotalBooks = totalBooks,
            TotalUsers = totalUsers,
            TotalReadings = totalReadings,
            CompletedReadings = completedReadings,
            InProgressReadings = inProgressReadings,
            NotStartedReadings = notStartedReadings,
            RecentActivities = recentActivities
        };
    }
}
