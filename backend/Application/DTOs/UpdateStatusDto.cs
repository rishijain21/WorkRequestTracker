using System.ComponentModel.DataAnnotations;

namespace WorkRequestTracker.Application.DTOs;

public class UpdateStatusDto
{
    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;
}
