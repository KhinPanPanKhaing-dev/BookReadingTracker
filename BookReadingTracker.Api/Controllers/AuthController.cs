using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var result = _authService.Login(login);
        if (result is null) return Unauthorized();
        return Ok(result);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto register)
    {
        var result = _authService.Register(register);
        if (result is null) return Conflict("Email already exists");
        return Ok(result);
    }
}
