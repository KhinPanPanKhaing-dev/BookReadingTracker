using BookReadingTracker.Domain.DTOs;

namespace BookReadingTracker.Domain.Interfaces;

public interface IDashboardService
{
    DashboardDto GetDashboard();
}
