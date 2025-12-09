namespace TaskManager.Application.DTOs;

/// <summary>
/// Data transfer object for updating a task
/// </summary>
public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int Priority { get; set; }
}

