using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Mvc.HttpClients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReadingTracker.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly AuthApiClient _authApi;

    public AuthController(AuthApiClient authApi)
    {
        _authApi = authApi;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto login)
    {
        if (!ModelState.IsValid) return View(login);

        var result = await _authApi.LoginAsync(login);
        if (result is null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(login);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.UserName),
            new(ClaimTypes.Role, result.RoleName),
            new("permission", string.Join(",", result.Permissions))
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        if (!ModelState.IsValid) return View(register);

        var result = await _authApi.RegisterAsync(register);
        if (result is null)
        {
            ModelState.AddModelError("", "Email already exists");
            return View(register);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.UserName),
            new(ClaimTypes.Role, result.RoleName),
            new("permission", string.Join(",", result.Permissions))
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
