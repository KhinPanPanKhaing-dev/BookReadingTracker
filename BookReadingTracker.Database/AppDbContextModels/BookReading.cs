using System;
using System.Collections.Generic;

namespace BookReadingTracker.Database.AppDbContextModels;

public partial class BookReading
{
    public int BookReadingId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public int CurrentPage { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
