using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Application.Interfaces;

public interface IWorkRequestRepository
{
    Task<(List<WorkRequest> Items, int TotalCount)> GetAllAsync(
        WorkRequestStatus? status, string? search, int page, int pageSize);

    Task<WorkRequest?> GetByIdAsync(Guid id);

    Task<WorkRequest?> GetByIdTrackedAsync(Guid id);

    Task<WorkRequest> CreateAsync(WorkRequest request);

    Task<WorkRequest> UpdateAsync(WorkRequest request);

    Task<Note> AddNoteAsync(Note note);

    Task<bool> ExistsAsync(Guid id);
}
