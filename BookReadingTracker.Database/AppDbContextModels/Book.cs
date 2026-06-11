using System;
using System.Collections.Generic;

namespace BookReadingTracker.Database.AppDbContextModels;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string? Description { get; set; }

    public int TotalPages { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<BookReading> BookReadings { get; set; } = new List<BookReading>();
}
