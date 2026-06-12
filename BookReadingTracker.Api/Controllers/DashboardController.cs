using BookReadingTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookReadingTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public IActionResult GetDashboard()
    {
        var dashboard = _dashboardService.GetDashboard();
        return Ok(dashboard);
    }
}
