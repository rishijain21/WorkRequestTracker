using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Application.Interfaces;
using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Infrastructure.Persistence.Repositories;

public class WorkRequestRepository : IWorkRequestRepository
{
    private readonly AppDbContext _context;

    public WorkRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<WorkRequest> Items, int TotalCount)> GetAllAsync(
        WorkRequestStatus? status, string? search, int page, int pageSize)
    {
        var query = _context.WorkRequests.AsQueryable();

        // Filter by status when provided
        if (status.HasValue)
        {
            query = query.Where(w => w.Status == status.Value);
        }

        // Search by title or client name (case-insensitive, partial match)
        // SQLite LIKE is case-insensitive for ASCII by default
        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(w =>
                EF.Functions.Like(w.Title, pattern) ||
                EF.Functions.Like(w.ClientName, pattern));
        }

        // Get total count before pagination (for PaginationMeta)
        var totalCount = await query.CountAsync();

        // Apply pagination with deterministic ordering
        // SQLite stores DateTimeOffset as TEXT (ISO 8601), so order by string representation
        var items = await query
            .OrderByDescending(w => w.CreatedAt.ToString())
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<WorkRequest?> GetByIdAsync(Guid id)
    {
        return await _context.WorkRequests
            .Include(w => w.Notes.OrderBy(n => n.CreatedAt.ToString()))
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<WorkRequest?> GetByIdTrackedAsync(Guid id)
    {
        return await _context.WorkRequests
            .Include(w => w.Notes)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<WorkRequest> CreateAsync(WorkRequest request)
    {
        _context.WorkRequests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<WorkRequest> UpdateAsync(WorkRequest request)
    {
        // Entity is already tracked — just save changes
        await _context.SaveChangesAsync();

        // Reload with notes (untracked) for the response
        return (await GetByIdAsync(request.Id))!;
    }

    public async Task<Note> AddNoteAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.WorkRequests.AnyAsync(w => w.Id == id);
    }
}
