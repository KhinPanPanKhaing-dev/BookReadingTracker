using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;

namespace BookReadingTracker.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public AuthResultDto? Login(LoginDto login)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == login.Email);
        if (user is null) return null;

        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            return null;

        return new AuthResultDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            RoleName = user.RoleName,
            Permissions = GetPermissionsForRole(user.RoleName)
        };
    }

    public AuthResultDto? Register(RegisterDto register)
    {
        if (_db.Users.Any(x => x.Email == register.Email))
            return null;

        var entity = new User
        {
            UserName = register.UserName,
            Email = register.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
            RoleName = "User",
            CreatedBy = register.UserName,
            CreatedDate = DateTime.UtcNow
        };

        _db.Users.Add(entity);
        _db.SaveChanges();

        return new AuthResultDto
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            RoleName = entity.RoleName,
            Permissions = GetPermissionsForRole("User")
        };
    }

    private static List<string> GetPermissionsForRole(string roleName)
    {
        if (roleName == "Admin")
        {
            return
            [
                "Book.Read", "Book.Create", "Book.Update", "Book.Delete",
                "User.Read", "User.Create", "User.Update", "User.Delete",
                "Reading.Read", "Reading.Create", "Reading.Update", "Reading.Delete",
                "Dashboard.View"
            ];
        }

        return
        [
            "Book.Read",
            "User.Read",
            "Reading.Read", "Reading.Create", "Reading.Update",
            "Dashboard.View"
        ];
    }
}
