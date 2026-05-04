using System.ComponentModel.DataAnnotations;

namespace WorkRequestTracker.Application.DTOs;

public class AddNoteDto
{
    [Required(ErrorMessage = "Content is required.")]
    [MinLength(1, ErrorMessage = "Content cannot be empty.")]
    public string Content { get; set; } = string.Empty;
}
