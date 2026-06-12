using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Domain.Interfaces;

public interface IAuthService
{
    AuthResultDto? Login(LoginDto login);
    AuthResultDto? Register(RegisterDto register);
}
