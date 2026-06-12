using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Domain.Interfaces;

public interface IUserService
{
    List<UserDto> GetUsers();
    UserDto? GetUser(int id);
    UserDto? CreateUser(string userName, string email, string password, string roleName);
    UserDto? UpdateUser(int id, string userName, string email, string roleName, string? password);
    bool DeleteUser(int id);
}
