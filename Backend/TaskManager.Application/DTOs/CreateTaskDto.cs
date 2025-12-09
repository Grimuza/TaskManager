namespace TaskManager.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new task
/// </summary>
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; } = 0; // Default: Low/Green
}

