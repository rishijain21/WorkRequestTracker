namespace WorkRequestTracker.Domain.Entities;

public class Note
{
    public Guid Id { get; set; }
    public Guid WorkRequestId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public WorkRequest WorkRequest { get; set; } = null!;
}
