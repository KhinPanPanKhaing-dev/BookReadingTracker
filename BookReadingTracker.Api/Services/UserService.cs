using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReadingTracker.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public List<UserDto> GetUsers()
    {
        return _db.Users
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new UserDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                RoleName = x.RoleName,
                PasswordHash = x.PasswordHash,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate
            })
            .ToList();
    }

    public UserDto? GetUser(int id)
    {
        var x = _db.Users.AsNoTracking().FirstOrDefault(u => u.UserId == id);
        if (x is null) return null;

        return new UserDto
        {
            UserId = x.UserId,
            UserName = x.UserName,
            Email = x.Email,
            RoleName = x.RoleName,
            PasswordHash = x.PasswordHash,
            CreatedBy = x.CreatedBy,
            CreatedDate = x.CreatedDate,
            ModifiedBy = x.ModifiedBy,
            ModifiedDate = x.ModifiedDate
        };
    }

    public UserDto? CreateUser(string userName, string email, string password, string roleName)
    {
        if (_db.Users.Any(x => x.Email == email))
            return null;

        var entity = new User
        {
            UserName = userName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleName = roleName,
            CreatedBy = "Admin",
            CreatedDate = DateTime.UtcNow
        };

        _db.Users.Add(entity);
        _db.SaveChanges();

        return new UserDto
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            Email = entity.Email,
            RoleName = entity.RoleName,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate
        };
    }

    public UserDto? UpdateUser(int id, string userName, string email, string roleName, string? password)
    {
        var entity = _db.Users.Find(id);
        if (entity is null) return null;

        entity.UserName = userName;
        entity.Email = email;
        entity.RoleName = roleName;
        if (!string.IsNullOrEmpty(password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        entity.ModifiedBy = "Admin";
        entity.ModifiedDate = DateTime.UtcNow;

        _db.SaveChanges();

        return new UserDto
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            Email = entity.Email,
            RoleName = entity.RoleName,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,
            ModifiedBy = entity.ModifiedBy,
            ModifiedDate = entity.ModifiedDate
        };
    }

    public bool DeleteUser(int id)
    {
        var entity = _db.Users.Include(x => x.BookReadings).FirstOrDefault(x => x.UserId == id);
        if (entity is null) return false;

        _db.BookReadings.RemoveRange(entity.BookReadings);
        _db.Users.Remove(entity);
        _db.SaveChanges();
        return true;
    }
}
