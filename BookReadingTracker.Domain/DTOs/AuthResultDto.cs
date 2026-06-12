namespace BookReadingTracker.Domain.DTOs;

public class AuthResultDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public List<string> Permissions { get; set; } = [];
}
