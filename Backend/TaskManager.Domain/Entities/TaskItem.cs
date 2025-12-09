namespace TaskManager.Domain.Entities;

/// <summary>
/// Represents a task item in the system
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int Order { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Task priority levels
/// </summary>
public enum TaskPriority
{
    Low = 0,      // Green
    Medium = 1,   // Yellow
    High = 2      // Red
}

