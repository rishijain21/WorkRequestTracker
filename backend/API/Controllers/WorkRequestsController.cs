using Microsoft.AspNetCore.Mvc;
using WorkRequestTracker.Application.Common;
using WorkRequestTracker.Application.DTOs;
using WorkRequestTracker.Application.Interfaces;

namespace WorkRequestTracker.API.Controllers;

[ApiController]
[Route("api/work-requests")]
public class WorkRequestsController : ControllerBase
{
    private readonly IWorkRequestService _service;

    public WorkRequestsController(IWorkRequestService service)
    {
        _service = service;
    }

    // GET /api/work-requests?status=InProgress&search=acme&page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(status, search, page, pageSize);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // GET /api/work-requests/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    // POST /api/work-requests
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    // PATCH /api/work-requests/{id}/status
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto);

        if (!result.Success)
        {
            if (result.Message == ApiMessages.NotFound)
                return NotFound(result);

            return BadRequest(result);
        }

        return Ok(result);
    }

    // POST /api/work-requests/{id}/notes
    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddNoteDto dto)
    {
        var result = await _service.AddNoteAsync(id, dto);

        if (!result.Success)
        {
            if (result.Message == ApiMessages.NotFound)
                return NotFound(result);

            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetById), new { id }, result);
    }
}
