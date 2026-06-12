namespace BookReadingTracker.Domain.DTOs;

public class BookReadingDto
{
    public int BookReadingId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int CurrentPage { get; set; }
    public string? Status { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? BookTitle { get; set; }
    public string? UserName { get; set; }
    public string? Author { get; set; }
    public int TotalPages { get; set; }
    public DateTime CreatedDate { get; set; }
}
