namespace BookReadingTracker.Domain.DTOs;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;
    public string? Author { get; set; }
    public string? Description { get; set; }
    public int TotalPages { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
