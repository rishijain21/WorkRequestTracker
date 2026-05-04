using WorkRequestTracker.Application.Common;
using WorkRequestTracker.Application.DTOs;

namespace WorkRequestTracker.Application.Interfaces;

public interface IWorkRequestService
{
    Task<ApiResponse<List<WorkRequestSummaryDto>>> GetAllAsync(
        string? status, string? search, int page, int pageSize);

    Task<ApiResponse<WorkRequestDto>> GetByIdAsync(Guid id);

    Task<ApiResponse<WorkRequestDto>> CreateAsync(CreateWorkRequestDto dto);

    Task<ApiResponse<WorkRequestDto>> UpdateStatusAsync(Guid id, UpdateStatusDto dto);

    Task<ApiResponse<NoteDto>> AddNoteAsync(Guid id, AddNoteDto dto);
}
