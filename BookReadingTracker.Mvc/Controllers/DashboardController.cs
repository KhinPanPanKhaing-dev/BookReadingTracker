using BookReadingTracker.Mvc.Filters;
using BookReadingTracker.Mvc.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Mvc.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly DashboardApiClient _dashboardApi;

    public DashboardController(DashboardApiClient dashboardApi)
    {
        _dashboardApi = dashboardApi;
    }

    [Permission("Dashboard.View")]
    public async Task<IActionResult> Index()
    {
        var dashboard = await _dashboardApi.GetDashboardAsync();
        return View(dashboard);
    }
}
