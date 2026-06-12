namespace BookReadingTracker.Domain.DTOs;

public class DashboardDto
{
    public int TotalBooks { get; set; }
    public int TotalUsers { get; set; }
    public int TotalReadings { get; set; }
    public int CompletedReadings { get; set; }
    public int InProgressReadings { get; set; }
    public int NotStartedReadings { get; set; }
    public List<BookReadingDto> RecentActivities { get; set; } = [];
}
