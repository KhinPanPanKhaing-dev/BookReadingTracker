using BookReadingTracker.Domain.DTOs;
using BookReadingTracker.Mvc.Filters;
using BookReadingTracker.Mvc.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Mvc.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly UserApiClient _userApi;

    public UsersController(UserApiClient userApi)
    {
        _userApi = userApi;
    }

    [Permission("User.Read")]
    public async Task<IActionResult> Index()
    {
        var users = await _userApi.GetUsersAsync();
        return View(users);
    }

    [Permission("User.Create")]
    public IActionResult Create()
    {
        return View();
    }

    [Permission("User.Create")]
    [HttpPost]
    public async Task<IActionResult> Create(string userName, string email, string password, string roleName)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "All fields are required");
            return View();
        }

        try
        {
            await _userApi.CreateUserAsync(userName, email, password, roleName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
            return View();
        }
    }

    [Permission("User.Update")]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userApi.GetUserAsync(id);
        if (user is null) return NotFound();
        return View(user);
    }

    [Permission("User.Update")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string userName, string email, string roleName, string? password)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError("", "Name and email are required");
            return View(await _userApi.GetUserAsync(id));
        }

        try
        {
            await _userApi.UpdateUserAsync(id, userName, email, roleName, password);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
            return View(await _userApi.GetUserAsync(id));
        }
    }

    [Permission("User.Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userApi.GetUserAsync(id);
        if (user is null) return NotFound();
        return View(user);
    }

    [Permission("User.Delete")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _userApi.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
