namespace BookReadingTracker.Domain.Constants;

public static class StatusConstants
{
    public const string NotStarted = "Not Started";
    public const string InProgress = "Reading";
    public const string Completed = "Completed";

    public static readonly string[] ValidStatuses = [NotStarted, InProgress, Completed];

    public static bool IsValid(string status)
    {
        return ValidStatuses.Contains(status);
    }
}
