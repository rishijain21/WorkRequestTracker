using WorkRequestTracker.Application.Common;
using WorkRequestTracker.Application.DTOs;
using WorkRequestTracker.Application.Interfaces;
using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Application.Services;

public class WorkRequestService : IWorkRequestService
{
    private readonly IWorkRequestRepository _repository;

    public WorkRequestService(IWorkRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<List<WorkRequestSummaryDto>>> GetAllAsync(
        string? status, string? search, int page, int pageSize)
    {
        // Validate status filter if provided
        WorkRequestStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<WorkRequestStatus>(status, ignoreCase: true, out var s))
            {
                return new ApiResponse<List<WorkRequestSummaryDto>>
                {
                    Success = false,
                    Message = "Invalid status value.",
                    Errors = new List<string> { $"'{status}' is not a valid status. Use: New, InProgress, Blocked, or Completed." }
                };
            }
            parsedStatus = s;
        }

        var (items, totalCount) = await _repository.GetAllAsync(parsedStatus, search, page, pageSize);

        var dtos = items.Select(MapToSummaryDto).ToList();

        return new ApiResponse<List<WorkRequestSummaryDto>>
        {
            Success = true,
            Data = dtos,
            Pagination = new PaginationMeta
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }
        };
    }

    public async Task<ApiResponse<WorkRequestDto>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.NotFound
            };
        }

        return new ApiResponse<WorkRequestDto>
        {
            Success = true,
            Data = MapToDto(entity)
        };
    }

    public async Task<ApiResponse<WorkRequestDto>> CreateAsync(CreateWorkRequestDto dto)
    {
        // Validate enum: Priority
        if (!Enum.TryParse<Priority>(dto.Priority, ignoreCase: true, out var priority))
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.ValidationFailed,
                Errors = new List<string> { "Invalid priority value." }
            };
        }

        // Validate enum: Status
        if (!Enum.TryParse<WorkRequestStatus>(dto.Status, ignoreCase: true, out var status))
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.ValidationFailed,
                Errors = new List<string> { "Invalid status value." }
            };
        }

        // Validate DueDate is in the future
        if (dto.DueDate!.Value <= DateTimeOffset.UtcNow)
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.ValidationFailed,
                Errors = new List<string> { "DueDate must be a future date." }
            };
        }

        var entity = new WorkRequest
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ClientName = dto.ClientName,
            Description = dto.Description,
            Priority = priority,
            Status = status,
            DueDate = dto.DueDate.Value
            // CreatedAt and UpdatedAt are set by AppDbContext.SaveChanges
        };

        var created = await _repository.CreateAsync(entity);

        return new ApiResponse<WorkRequestDto>
        {
            Success = true,
            Message = "Work request created.",
            Data = MapToDto(created)
        };
    }

    public async Task<ApiResponse<WorkRequestDto>> UpdateStatusAsync(Guid id, UpdateStatusDto dto)
    {
        // Validate enum
        if (!Enum.TryParse<WorkRequestStatus>(dto.Status, ignoreCase: true, out var newStatus))
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.ValidationFailed,
                Errors = new List<string> { "Invalid status value." }
            };
        }

        var entity = await _repository.GetByIdTrackedAsync(id);

        if (entity is null)
        {
            return new ApiResponse<WorkRequestDto>
            {
                Success = false,
                Message = ApiMessages.NotFound
            };
        }

        entity.Status = newStatus;
        // UpdatedAt is set by AppDbContext.SaveChanges

        var updated = await _repository.UpdateAsync(entity);

        return new ApiResponse<WorkRequestDto>
        {
            Success = true,
            Message = "Status updated.",
            Data = MapToDto(updated)
        };
    }

    public async Task<ApiResponse<NoteDto>> AddNoteAsync(Guid id, AddNoteDto dto)
    {
        var exists = await _repository.ExistsAsync(id);

        if (!exists)
        {
            return new ApiResponse<NoteDto>
            {
                Success = false,
                Message = ApiMessages.NotFound
            };
        }

        var note = new Note
        {
            Id = Guid.NewGuid(),
            WorkRequestId = id,
            Content = dto.Content
            // CreatedAt is set by AppDbContext.SaveChanges
        };

        var created = await _repository.AddNoteAsync(note);

        return new ApiResponse<NoteDto>
        {
            Success = true,
            Message = "Note added.",
            Data = MapToNoteDto(created)
        };
    }

    // Mapping helpers

    private static WorkRequestSummaryDto MapToSummaryDto(WorkRequest entity)
    {
        return new WorkRequestSummaryDto
        {
            Id = entity.Id,
            Title = entity.Title,
            ClientName = entity.ClientName,
            Priority = entity.Priority.ToString(),
            Status = entity.Status.ToString(),
            DueDate = entity.DueDate,
            CreatedAt = entity.CreatedAt
        };
    }

    private static WorkRequestDto MapToDto(WorkRequest entity)
    {
        return new WorkRequestDto
        {
            Id = entity.Id,
            Title = entity.Title,
            ClientName = entity.ClientName,
            Description = entity.Description,
            Priority = entity.Priority.ToString(),
            Status = entity.Status.ToString(),
            DueDate = entity.DueDate,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Notes = entity.Notes?.Select(MapToNoteDto).ToList() ?? new List<NoteDto>()
        };
    }

    private static NoteDto MapToNoteDto(Note entity)
    {
        return new NoteDto
        {
            Id = entity.Id,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt
        };
    }
}
