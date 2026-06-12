using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _userService.GetUser(id);
        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var created = _userService.CreateUser(request.UserName, request.Email, request.Password, request.RoleName);
        if (created is null) return Conflict("Email already exists");
        return CreatedAtAction(nameof(GetUser), new { id = created.UserId }, created);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var updated = _userService.UpdateUser(id, request.UserName, request.Email, request.RoleName, request.Password);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var deleted = _userService.DeleteUser(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

public class CreateUserRequest
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RoleName { get; set; } = "User";
}

public class UpdateUserRequest
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string? Password { get; set; }
}
