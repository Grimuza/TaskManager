namespace TaskManager.Application.DTOs;

/// <summary>
/// Data transfer object for reordering tasks
/// </summary>
public class ReorderTaskDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
}

