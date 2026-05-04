using System.ComponentModel.DataAnnotations;

namespace WorkRequestTracker.Application.DTOs;

public class CreateWorkRequestDto
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "ClientName is required.")]
    [MaxLength(150, ErrorMessage = "ClientName must not exceed 150 characters.")]
    public string ClientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required.")]
    public string Priority { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "DueDate is required.")]
    public DateTimeOffset? DueDate { get; set; }
}
