using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Domain.Entities;

public class WorkRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public WorkRequestStatus Status { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Note> Notes { get; set; } = new();
}
